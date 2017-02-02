using System;
using Untech.Practices.CQRS.Requests;

namespace Untech.Practices.CQRS.Dispatching
{
	public sealed class NoQueuedDispatcher : IQueuedDispatcher
	{
		private IDispatcher _parent;

		public void Init(IDispatcher parent)
		{
			_parent = parent;
		}

		public void Enqueue<TResponse>(ICommand<TResponse> command, QueueOptions options)
		{
			if (_parent == null) throw new InvalidOperationException("Not initialized");
			_parent.Process(command);
		}

		public void Enqueue(INotification notification, QueueOptions options)
		{
			if (_parent == null) throw new InvalidOperationException("Not initialized");
			_parent.Publish(notification);
		}
	}
}