using System;
using System.Threading.Tasks;

namespace Untech.Practices.CQRS.Dispatching
{
	/// <summary>
	/// Implements <see cref="IQueueDispatcher"/> with immediate queue.
	/// </summary>
	public sealed class SimpleQueueDispatcher : IQueueDispatcher
	{
		private IDispatcher _parent;

		public QueueOptions DefaultOptions { get; set; } = QueueOptions.CreateDefault();

		/// <inheritdoc />
		public void Init(IDispatcher parent)
		{
			_parent = parent;
		}

		/// <inheritdoc />
		public void Enqueue<TResponse>(ICommand<TResponse> command, QueueOptions options = null)
		{
			if (command == null) throw new ArgumentNullException(nameof(command));

			CheckInitialized();
			Task.Factory.StartNew(() => _parent.Process(command));
		}

		/// <inheritdoc />
		public void Enqueue(INotification notification, QueueOptions options = null)
		{
			if (notification == null) throw new ArgumentNullException(nameof(notification));

			CheckInitialized();
			Task.Factory.StartNew(() => _parent.Publish(notification));
		}

		private void CheckInitialized()
		{
			if (_parent != null) return;
			throw new InvalidOperationException("Not initialized");
		}
	}
}