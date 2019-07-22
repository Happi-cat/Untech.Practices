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
		public Task EnqueueAsync(INotification notification, CancellationToken cancellationToken,
			QueueOptions options = null)
		{
			if (notification == null) throw new ArgumentNullException(nameof(notification));

			return _parent.PublishAsync(notification, cancellationToken);
		}

		public async Task EnqueueAsync(IEnumerable<INotification> notifications, CancellationToken cancellationToken,
			QueueOptions options = null)
		{
			if (notifications == null) throw new ArgumentNullException(nameof(notifications));

			foreach (var notification in notifications) await EnqueueAsync(notification, cancellationToken, options);
		}
	}
}