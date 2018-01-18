using System;
using System.Threading.Tasks;

namespace Untech.Practices.CQRS.Dispatching
{
	/// <summary>
	/// Implements <see cref="IQueueDispatcher"/> with immediate queue.
	/// </summary>
	public sealed class InlineQueueDispatcher : IQueueDispatcher
	{
		private readonly IDispatcher _parent;

		public InlineQueueDispatcher(IDispatcher parent)
		{
			_parent = parent ?? throw new ArgumentNullException(nameof(parent));
		}

		/// <inheritdoc />
		public void Enqueue<TResponse>(ICommand<TResponse> command, QueueOptions options = null)
		{
			if (command == null) throw new ArgumentNullException(nameof(command));

			_parent.Process(command);
		}

		/// <inheritdoc />
		public void Enqueue(INotification notification, QueueOptions options = null)
		{
			if (notification == null) throw new ArgumentNullException(nameof(notification));

			_parent.Publish(notification);
		}
	}
}