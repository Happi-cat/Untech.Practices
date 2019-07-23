using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.CQRS.Dispatching
{
	/// <summary>
	///     Implements <see cref="IQueueDispatcher" /> with immediate queue.
	/// </summary>
	public sealed class InlineQueueDispatcher : IQueueDispatcher
	{
		private readonly IDispatcher _parent;

		public InlineQueueDispatcher(IDispatcher parent)
		{
			_parent = parent ?? throw new ArgumentNullException(nameof(parent));
		}

		/// <inheritdoc />
		public Task EnqueueAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default,
			QueueOptions options = null)
		{
			if (command == null) throw new ArgumentNullException(nameof(command));

			return _parent.ProcessAsync(command, cancellationToken);
		}

		/// <inheritdoc />
		public Task EnqueueAsync(IEvent @event, CancellationToken cancellationToken,
			QueueOptions options = null)
		{
			if (@event == null) throw new ArgumentNullException(nameof(@event));

			return _parent.PublishAsync(@event, cancellationToken);
		}

		public async Task EnqueueAsync(IEnumerable<IEvent> events, CancellationToken cancellationToken,
			QueueOptions options = null)
		{
			if (events == null) throw new ArgumentNullException(nameof(events));

			foreach (var notification in events) await EnqueueAsync(notification, cancellationToken, options);
		}
	}
}