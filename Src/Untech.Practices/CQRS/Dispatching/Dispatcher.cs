using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Untech.Practices.CQRS.Dispatching.RequestExecutors;

namespace Untech.Practices.CQRS.Dispatching
{
	/// <summary>
	/// Represents class that implements <see cref="IDispatcher"/> interface 
	/// and dispatches incoming CQRS request to appropriate handlers.
	/// </summary>
	public sealed class Dispatcher : IDispatcher
	{
		private readonly IHandlersResolver _handlersResolver;
		private readonly ConcurrentDictionary<Type, IRequestExecutor> _executors;

		/// <summary>
		/// Initializes a new instance of the <see cref="Dispatcher"/>.
		/// </summary>
		/// <param name="handlersResolver">The resolver of CQRS handlers.</param>
		/// <param name="queuedDispatcher">Dispatcher that enqueues CQRS requests. 
		///    <see cref="SimpleQueueDispatcher"/> will be used when null is passed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="handlersResolver"/> is null.</exception>
		public Dispatcher(IHandlersResolver handlersResolver, IQueueDispatcher queuedDispatcher = null)
		{
			Guard.CheckNotNull(nameof(handlersResolver), handlersResolver);

			_handlersResolver = new HandlersResolverWrapper(handlersResolver);
			_executors = new ConcurrentDictionary<Type, IRequestExecutor>();

			Queue = queuedDispatcher ?? new SimpleQueueDispatcher();
			Queue.Init(this);
		}

		/// <inheritdoc />
		public IQueueDispatcher Queue { get; }

		/// <inheritdoc />
		public TResponse Fetch<TResponse>(IQuery<TResponse> query)
		{
			Guard.CheckNotNull(nameof(query), query);

			return (TResponse)_executors
				.GetOrAdd(query.GetType(), MakeFetch<TResponse>)
				.Handle(query);
		}

		/// <inheritdoc />
		public Task<TResponse> FetchAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken)
		{
			Guard.CheckNotNull(nameof(query), query);

			return (Task<TResponse>)_executors
				.GetOrAdd(query.GetType(), MakeFetch<TResponse>)
				.HandleAsync(query, cancellationToken);
		}

		/// <inheritdoc />
		public TResponse Process<TResponse>(ICommand<TResponse> command)
		{
			Guard.CheckNotNull(nameof(command), command);

			return (TResponse)_executors
				.GetOrAdd(command.GetType(), MakeProcess<TResponse>)
				.Handle(command);
		}

		/// <inheritdoc />
		public Task<TResponse> ProcessAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken)
		{
			Guard.CheckNotNull(nameof(command), command);

			return (Task<TResponse>)_executors
				.GetOrAdd(command.GetType(), MakeProcess<TResponse>)
				.HandleAsync(command, cancellationToken);
		}

		/// <inheritdoc />
		public void Publish(INotification notification)
		{
			Guard.CheckNotNull(nameof(notification), notification);

			_executors
				.GetOrAdd(notification.GetType(), MakePublish)
				.Handle(notification);
		}

		/// <inheritdoc />
		public Task PublishAsync(INotification notification, CancellationToken cancellationToken)
		{
			Guard.CheckNotNull(nameof(notification), notification);

			return _executors
				.GetOrAdd(notification.GetType(), MakePublish)
				.HandleAsync(notification, cancellationToken);
		}

		#region [Private Methods]

		private IRequestExecutor MakeFetch<TResponse>(Type type)
		{
			return (IRequestExecutor)MakeExecutor(typeof(QueryRequestExecutor<,>), type, typeof(TResponse));
		}

		private IRequestExecutor MakeProcess<TResponse>(Type type)
		{
			return (IRequestExecutor)MakeExecutor(typeof(CommandRequestExecutor<,>), type, typeof(TResponse));
		}

		private IRequestExecutor MakePublish(Type type)
		{
			return (IRequestExecutor)MakeExecutor(typeof(NotificationRequestExecutor<>), type);
		}

		private object MakeExecutor(Type genericExecutorType, Type requestType, Type responseType)
		{
			var executorType = genericExecutorType.MakeGenericType(requestType, responseType);
			return Activator.CreateInstance(executorType, new object[] { _handlersResolver }, null);
		}

		private object MakeExecutor(Type genericExecutorType, Type requestType)
		{
			var executorType = genericExecutorType.MakeGenericType(requestType);
			return Activator.CreateInstance(executorType, new object[] { _handlersResolver }, null);
		}

		#endregion
	}
}