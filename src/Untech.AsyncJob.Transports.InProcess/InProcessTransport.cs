using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Untech.Practices.CQRS;
using Untech.Practices.CQRS.Dispatching;

namespace Untech.AsyncJob.Transports.InProcess
{
	public class InProcessTransport : ITransport, IQueueDispatcher
	{
		private readonly IDictionary<int, Queue> _queues;
		public InProcessTransport()
		{
			var scaleFactor = 10;
			_queues = new [] { -1, 0, 1}
				.ToDictionary(n => n * scaleFactor, n => new Queue(), new PriorityComparer(scaleFactor));
		}

		public Task<ReadOnlyCollection<Request>> GetRequestsAsync(int count)
		{
			var maxCountPerPriorityQueue = Math.Max(count - 2, 1);
			var requests = _queues
				.OrderByDescending(n => n.Key)
				.SelectMany(n => n.Value.Dequeue(maxCountPerPriorityQueue))
				.Take(count)
				.OrderByDescending(r => r.GetPriority())
				.ToList<Request>()
				.AsReadOnly();

			return Task.FromResult(requests);
		}

		public async Task CompleteRequestAsync(Request request)
		{
		}

		public async Task FailRequestAsync(Request request, Exception exception)
		{
		}

		public Task EnqueueAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default,
			QueueOptions options = null)
		{
			var request = new InProcessRequest(command, options);
			_queues[request.GetPriority()].Enqueue(request);
			return Task.CompletedTask;
		}

		public Task EnqueueAsync(INotification notification, CancellationToken cancellationToken = default,
			QueueOptions options = null)
		{
			var request = new InProcessRequest(notification, options);
			_queues[request.GetPriority()].Enqueue(request);
			return Task.CompletedTask;
		}

		/// <summary>
		/// Assumes 3 priority groups: -1, 0, 1. Divides numbers into groups according to scale factor.
		/// </summary>
		private class PriorityComparer : IEqualityComparer<int>
		{
			private readonly int _factor;

			public PriorityComparer(int scaleFactor = 1)
			{
				_factor = Math.Max(scaleFactor, 1);
			}

			public bool Equals(int x, int y)
			{
				return Normalize(x) == Normalize(y);
			}

			public int GetHashCode(int obj)
			{
				return Normalize(obj).GetHashCode();
			}

			private int Normalize(int value)
			{
				value /= _factor;
				value = Math.Max(value, -1);
				return Math.Min(value, 1);
			}
		}

		private class Queue
		{
			private readonly ConcurrentQueue<InProcessRequest> _queue;
			private readonly ConcurrentQueue<InProcessRequest> _delayQueue;

			public Queue()
			{
				_queue = new ConcurrentQueue<InProcessRequest>();
				_delayQueue = new ConcurrentQueue<InProcessRequest>();
			}

			public IEnumerable<InProcessRequest> Dequeue(int count)
			{
				return Delayed(count).Concat(Main(count)).Take(count);
			}

			public void Enqueue(InProcessRequest item)
			{
				_queue.Enqueue(item);
			}

			private IEnumerable<InProcessRequest> Delayed(int count)
			{
				count = Math.Max(count / 2, 1);
				return TryDeque(_delayQueue, _delayQueue, count);
			}

			private IEnumerable<InProcessRequest> Main(int count)
			{
				return TryDeque(_queue, _delayQueue, count * 2);
			}

			private static IEnumerable<InProcessRequest> TryDeque(
				ConcurrentQueue<InProcessRequest> queue,
				ConcurrentQueue<InProcessRequest> delayQueue,
				int maxAttempts)
			{
				while (maxAttempts-- > 0)
				{
					if (!queue.TryDequeue(out var request)) break;

					if (request.IsExpired()) continue;
					if (request.IsExecuteAfterReached()) yield return request;
					else delayQueue.Enqueue(request);
				}
			}
		}
	}
}
