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
		private readonly ITypeResolver _typeResolver;
		private readonly ITypeInitializer _typeInitializer;
		private readonly ConcurrentDictionary<Type, IHandlerRunner> _handlerRunners;

		/// <summary>
		/// Initializes a new instance of the <see cref="Dispatcher" />.
		/// </summary>
		/// <param name="typeResolver">The resolver of CQRS handlers.</param>
		/// <param name="typeInitializer">The handler post initializer.</param>
		/// <exception cref="ArgumentNullException"><paramref name="typeResolver" /> is null.</exception>
		public Dispatcher(ITypeResolver typeResolver, ITypeInitializer typeInitializer = null)
		{
			_typeResolver = typeResolver ?? throw new ArgumentNullException(nameof(typeResolver));
			_typeInitializer = typeInitializer;
			_handlerRunners = new ConcurrentDictionary<Type, IHandlerRunner>();
		}

		/// <inheritdoc />
		public TResponse Fetch<TResponse>(IQuery<TResponse> query)
		{
			query = query ?? throw new ArgumentNullException(nameof(query));

			return (TResponse)_handlerRunners
				.GetOrAdd(query.GetType(), MakeFetch<TResponse>)
				.Handle(query);
		}

		/// <inheritdoc />
		public Task<TResponse> FetchAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken)
		{
			query = query ?? throw new ArgumentNullException(nameof(query));

			return (Task<TResponse>)_handlerRunners
				.GetOrAdd(query.GetType(), MakeFetch<TResponse>)
				.HandleAsync(query, cancellationToken);
		}

		/// <inheritdoc />
		public TResponse Process<TResponse>(ICommand<TResponse> command)
		{
			command = command ?? throw new ArgumentNullException(nameof(command));

			return (TResponse)_handlerRunners
				.GetOrAdd(command.GetType(), MakeProcess<TResponse>)
				.Handle(command);
		}

		/// <inheritdoc />
		public Task<TResponse> ProcessAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken)
		{
			command = command ?? throw new ArgumentNullException(nameof(command));

			return (Task<TResponse>)_handlerRunners
				.GetOrAdd(command.GetType(), MakeProcess<TResponse>)
				.HandleAsync(command, cancellationToken);
		}

		/// <inheritdoc />
		public void Publish(INotification notification)
		{
			notification = notification ?? throw new ArgumentNullException(nameof(notification));

			_handlerRunners
				.GetOrAdd(notification.GetType(), MakePublish)
				.Handle(notification);
		}

		/// <inheritdoc />
		public Task PublishAsync(INotification notification, CancellationToken cancellationToken)
		{
			notification = notification ?? throw new ArgumentNullException(nameof(notification));

			return _handlerRunners
				.GetOrAdd(notification.GetType(), MakePublish)
				.HandleAsync(notification, cancellationToken);
		}

		#region [Private Methods]

		private IHandlerRunner MakeFetch<TResponse>(Type type)
		{
			return (IHandlerRunner)CreateHandlerRunner(typeof(QueryHandlerRunner<,>), type, typeof(TResponse));
		}

		private IHandlerRunner MakeProcess<TResponse>(Type type)
		{
			return (IHandlerRunner)CreateHandlerRunner(typeof(CommandHandlerRunner<,>), type, typeof(TResponse));
		}

		private IHandlerRunner MakePublish(Type type)
		{
			return (IHandlerRunner)CreateHandlerRunner(typeof(NotificationHandlerRunner<>), type);
		}

		private object CreateHandlerRunner(Type genericExecutorType, Type requestType, Type responseType)
		{
			var executorType = genericExecutorType.MakeGenericType(requestType, responseType);
			return Activator.CreateInstance(executorType, new object[] { _typeResolver, _typeInitializer }, null);
		}

		private object CreateHandlerRunner(Type genericExecutorType, Type requestType)
		{
			var executorType = genericExecutorType.MakeGenericType(requestType);
			return Activator.CreateInstance(executorType, new object[] { _typeResolver, _typeInitializer }, null);
		}

		#endregion
	}
}