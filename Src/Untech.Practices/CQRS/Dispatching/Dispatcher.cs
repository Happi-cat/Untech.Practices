using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Untech.Practices.CQRS.Dispatching.RequestExecutors;
using Untech.Practices.CQRS.Requests;

namespace Untech.Practices.CQRS.Dispatching
{
	public sealed class Dispatcher : IDispatcher
	{
		private readonly IHandlersResolver _handlersResolver;
		private readonly IQueuedDispatcher _queuedDispatcher;
		private readonly ConcurrentDictionary<Type, IRequestExecutor> _executors;

		public Dispatcher(IHandlersResolver handlersResolver, IQueuedDispatcher queuedDispatcher = null)
		{
			Guard.CheckNotNull(nameof(handlersResolver), handlersResolver);

			_handlersResolver = new HandlersResolverWrapper(handlersResolver);
			_queuedDispatcher = queuedDispatcher ?? new NoQueuedDispatcher();

			_executors = new ConcurrentDictionary<Type, IRequestExecutor>();

			_queuedDispatcher.Init(this);
		}

		public TResponse Fetch<TResponse>(IQuery<TResponse> query)
		{
			Guard.CheckNotNull(nameof(query), query);

			return (TResponse)_executors
				.GetOrAdd(query.GetType(), MakeFetch<TResponse>)
				.Handle(query);
		}

		public Task<TResponse> FetchAsync<TResponse>(IQuery<TResponse> query)
		{
			Guard.CheckNotNull(nameof(query), query);

			return (Task<TResponse>)_executors
				.GetOrAdd(query.GetType(), MakeFetch<TResponse>)
				.HandleAsync(query);
		}

		public TResponse Process<TResponse>(ICommand<TResponse> command)
		{
			Guard.CheckNotNull(nameof(command), command);

			return (TResponse)_executors
				.GetOrAdd(command.GetType(), MakeProcess<TResponse>)
				.Handle(command);
		}

		public Task<TResponse> ProcessAsync<TResponse>(ICommand<TResponse> command)
		{
			Guard.CheckNotNull(nameof(command), command);

			return (Task<TResponse>)_executors
				.GetOrAdd(command.GetType(), MakeProcess<TResponse>)
				.HandleAsync(command);
		}

		public void Publish(INotification notification)
		{
			Guard.CheckNotNull(nameof(notification), notification);

			_executors
				.GetOrAdd(notification.GetType(), MakePublish)
				.Handle(notification);
		}

		public Task PublishAsync(INotification notification)
		{
			Guard.CheckNotNull(nameof(notification), notification);

			return _executors
				.GetOrAdd(notification.GetType(), MakePublish)
				.HandleAsync(notification);
		}

		public void Enqueue<TResponse>(ICommand<TResponse> command, QueueOptions options)
		{
			Guard.CheckNotNull(nameof(command), command);

			_queuedDispatcher.Enqueue(command, options ?? QueueOptions.Default);
		}

		public void Enqueue(INotification notification, QueueOptions options)
		{
			Guard.CheckNotNull(nameof(notification), notification);

			_queuedDispatcher.Enqueue(notification, options ?? QueueOptions.Default);
		}

		#region [Private Methods]

		private object MakeExecutor<TResponse>(Type genericExecutorType, Type requestType)
		{
			var executorType = genericExecutorType.MakeGenericType(requestType, typeof(TResponse));
			return Activator.CreateInstance(executorType, new object[] { _handlersResolver }, null);
		}
		private object MakeExecutor(Type genericExecutorType, Type requestType)
		{
			var executorType = genericExecutorType.MakeGenericType(requestType);
			return Activator.CreateInstance(executorType, new object[] { _handlersResolver }, null);
		}

		private IRequestExecutor MakeFetch<TResponse>(Type type)
		{
			return (IRequestExecutor)MakeExecutor<TResponse>(typeof(QueryRequestExecutor<,>), type);
		}

		private IRequestExecutor MakeProcess<TResponse>(Type type)
		{
			return (IRequestExecutor)MakeExecutor<TResponse>(typeof(CommandRequestExecutor<,>), type);
		}

		private IRequestExecutor MakePublish(Type type)
		{
			return (IRequestExecutor) MakeExecutor(typeof(NotificationRequestExecutor<>), type);
		}

		#endregion
	}
}