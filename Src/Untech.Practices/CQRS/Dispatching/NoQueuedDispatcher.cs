using System;

namespace Untech.Practices.CQRS.Dispatching
{
	/// <summary>
	/// Implements <see cref="IQueuedDispatcher"/> with immediate queue.
	/// </summary>
	public sealed class NoQueuedDispatcher : IQueuedDispatcher
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
			CheckInitialized();
			_parent.Process(command);
		}

		/// <inheritdoc />
		public void Enqueue(INotification notification, QueueOptions options = null)
		{
			CheckInitialized();
			_parent.Publish(notification);
		}

		private void CheckInitialized()
		{
			if (_parent != null) return;
			throw new InvalidOperationException("Not initialized");
		}
	}
}