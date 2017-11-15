using System;
using System.Threading.Tasks;

namespace Untech.Practices.CQRS.Dispatching
{
	/// <summary>
	/// Implements <see cref="IQueueDispatcher"/> with immediate queue.
	/// </summary>
	public sealed class SimpleQueueDispatcher : IQueueDispatcher
	{
		private readonly IDispatcher _parent;

		public QueueOptions DefaultOptions { get; set; } = QueueOptions.CreateDefault();

		public SimpleQueueDispatcher(IDispatcher parent)
		{
			_parent = parent ?? throw new ArgumentNullException(nameof(parent));
		}

		/// <inheritdoc />
		public void Enqueue<TResponse>(ICommand<TResponse> command, QueueOptions options = null)
		{
			if (command == null) throw new ArgumentNullException(nameof(command));

			Task.Factory.StartNew(() => _parent.Process(command));
		}

		/// <inheritdoc />
		public void Enqueue(INotification notification, QueueOptions options = null)
		{
			if (notification == null) throw new ArgumentNullException(nameof(notification));

			Task.Factory.StartNew(() => _parent.Publish(notification));
		}
	}
}