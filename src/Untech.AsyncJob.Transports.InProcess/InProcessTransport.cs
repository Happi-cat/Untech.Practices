using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Untech.Practices.CQRS;
using Untech.Practices.CQRS.Dispatching;
using static System.Math;

namespace Untech.AsyncJob.Transports.InProcess
{
	public class InProcessTransport : ITransport, IQueueDispatcher
	{
		private readonly IDictionary<int, Queue> _queues;

		public InProcessTransport() : this(10, 1)
		{
		}

		private InProcessTransport(int scale, int radius)
		{
			_queues = Enumerable.Range(-radius, (radius << 1) | 1)
				.ToDictionary(n => n << scale, n => new Queue(), new PriorityComparer(scale, radius));
		}

		public Task<ReadOnlyCollection<Request>> GetRequestsAsync(int count)
		{
			var maxCountPerPriorityQueue = Max(count - 2, 1);
			var requests = _queues
				.OrderByDescending(n => n.Key)
				.SelectMany(n => n.Value.Dequeue(maxCountPerPriorityQueue))
				.Take(count)
				.OrderByDescending(r => r.GetPriority())
				.ToList<Request>()
				.AsReadOnly();

			return Task.FromResult(requests);
		}

		public Task CompleteRequestAsync(Request request)
		{
			return Task.CompletedTask;
		}

		public Task FailRequestAsync(Request request, Exception exception)
		{
			return Task.CompletedTask;
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
		/// Assumes we have priority groups: -r..r. Where group size is 1 &lt;&lt; scale
		/// </summary>
		private class PriorityComparer : IEqualityComparer<int>
		{
			private readonly int _radius;
			private readonly int _scale;

			public PriorityComparer(int scale = 0, int radius = 0)
			{
				_radius = Max(radius, 0);
				_scale = Max(scale, 0);
			}

			public bool Equals(int x, int y) => Normalize(x) == Normalize(y);

			public int GetHashCode(int obj) => Normalize(obj).GetHashCode();

			private int Normalize(int value) => Max(-_radius, Min(value >> _scale, _radius));
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
				count = Max(count >> 1, 1);
				return TryDeque(_delayQueue, _delayQueue, count);
			}

			private IEnumerable<InProcessRequest> Main(int count)
			{
				return TryDeque(_queue, _delayQueue, count << 1);
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
