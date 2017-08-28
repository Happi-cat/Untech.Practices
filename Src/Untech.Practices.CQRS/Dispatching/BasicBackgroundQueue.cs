using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Untech.Practices.CQRS.Dispatching
{
	public sealed class BasicBackgroundQueue : IQueueDispatcher
	{
		private const int MinDelay = 2000;
		private const int MaxBatchSize = 50;

		[Flags]
		private enum QueueItemExecutionResult
		{
			Success = 0x0,
			Failure = 0x1,
			Retry = 0x2
		}

		private readonly ConcurrentQueue<QueuedItem> _messages = new ConcurrentQueue<QueuedItem>();
		private readonly EventWaitHandle _waitHandle = new ManualResetEvent(false);
		private IDispatcher _parent;

		public QueueOptions DefaultOptions { get; set; } = QueueOptions.CreateDefault();

		public void Init(IDispatcher parent)
		{
			_parent = parent;

			ThreadPool.QueueUserWorkItem(DoWorkContinuously);
		}

		public void Enqueue<TResponse>(ICommand<TResponse> command, QueueOptions options)
		{
			_messages.Enqueue(QueuedItem.Create(command, options ?? DefaultOptions));
			_waitHandle.Set();
		}

		public void Enqueue(INotification notification, QueueOptions options)
		{
			_messages.Enqueue(QueuedItem.Create(notification, options ?? DefaultOptions));
			_waitHandle.Set();
		}

		private void DoWorkContinuously(object state)
		{
			var sw = new Stopwatch();
			while (true)
			{
				while (_messages.Count == 0) _waitHandle.WaitOne();

				sw.Start();
				new QueueProcessor(_messages, _parent).DoWork();
				sw.Stop();

				Delay(Math.Max(0, MinDelay - (int)sw.ElapsedMilliseconds));
			}
		}

		private static void Delay(int milliseconds)
		{
			if (milliseconds > 0)
				Thread.Sleep(milliseconds);
		}

		#region [Queued Item]

		private abstract class QueuedItem
		{
			protected QueuedItem(QueueOptions options)
			{
				Options = options ?? QueueOptions.CreateDefault();
				TimeToLive = options.TimeToLive;

				Created = DateTime.UtcNow;
				ExecuteAfter = options.ExecuteAfter != null ? (DateTime?)(Created + options.ExecuteAfter.Value) : null;
				ExpiresAfter = options.ExpiresAfter != null ? (DateTime?)(Created + options.ExpiresAfter.Value) : null;
			}

			public QueueOptions Options { get; }

			public int TimeToLive { get; set; }

			public DateTime Created { get; }

			public DateTime? ExpiresAfter { get; }

			public DateTime? ExecuteAfter { get; }

			public static QueuedItem Create<TResponse>(ICommand<TResponse> command, QueueOptions options)
			{
				return new QueuedCommand<TResponse>(command, options);
			}

			public static QueuedItem Create(INotification notification, QueueOptions options)
			{
				return new QueuedNotification(notification, options);
			}

			public abstract QueueItemExecutionResult Execute(IDispatcher dispatcher);
		}

		private class QueuedCommand<TResponse> : QueuedItem
		{
			private readonly ICommand<TResponse> _command;

			public QueuedCommand(ICommand<TResponse> command, QueueOptions options) : base(options)
			{
				_command = command;
			}

			public override QueueItemExecutionResult Execute(IDispatcher dispatcher)
			{
				dispatcher.Process(_command);
				return QueueItemExecutionResult.Success;
			}
		}

		private class QueuedNotification : QueuedItem
		{
			private readonly INotification _notification;

			public QueuedNotification(INotification notification, QueueOptions options) : base(options)
			{
				_notification = notification;
			}

			public override QueueItemExecutionResult Execute(IDispatcher dispatcher)
			{
				dispatcher.Publish(_notification);

				return QueueItemExecutionResult.Success;
			}
		}

		#endregion

		#region [Queue Processor]

		private class QueueProcessor
		{
			private readonly IProducerConsumerCollection<QueuedItem> _messages;
			private readonly IDispatcher _dispatcher;
			private readonly DateTime _clock = DateTime.UtcNow;

			public QueueProcessor(IProducerConsumerCollection<QueuedItem> messages, IDispatcher dispatcher)
			{
				_messages = messages;
				_dispatcher = dispatcher;
			}

			public void DoWork()
			{
				var messages = DequeMessages()
					.Where(n => n.ExpiresAfter == null || n.ExpiresAfter > _clock)
					.OrderByDescending(n => n.Options.Priority)
					.ThenBy(n => n.Created);

				HandleMessages(messages);
			}

			private IEnumerable<QueuedItem> DequeMessages()
			{
				var counter = MaxBatchSize;
				var messagesToDelay = new List<QueuedItem>();
				var messagesToProcess = new List<QueuedItem>();

				while (counter-- > 0 && _messages.TryTake(out QueuedItem message))
				{
					if (message.ExecuteAfter > _clock)
					{
						messagesToDelay.Add(message);
					}
					else
					{
						messagesToProcess.Add(message);
					}
				}

				messagesToDelay
					.OrderBy(n => n.Options.ExecuteAfter)
					.ToList()
					.ForEach(AddForDelay);

				return messagesToProcess;
			}

			private void HandleMessages(IEnumerable<QueuedItem> messages)
			{
				foreach (var message in messages)
				{
					if ((TryHandleMessage(message) & QueueItemExecutionResult.Retry) != QueueItemExecutionResult.Retry) continue;

					AddForRerty(message);
				}
			}

			private void AddForDelay(QueuedItem messageToDelay)
			{
				_messages.TryAdd(messageToDelay);
			}

			private void AddForRerty(QueuedItem message)
			{
				message.TimeToLive--;
				if (message.TimeToLive > -1)
					_messages.TryAdd(message);
			}

			private QueueItemExecutionResult TryHandleMessage(QueuedItem message)
			{
				try
				{
					return message.Execute(_dispatcher);
				}
				catch
				{
					return QueueItemExecutionResult.Failure & QueueItemExecutionResult.Retry;
				}
			}
		}

		#endregion
	}
}