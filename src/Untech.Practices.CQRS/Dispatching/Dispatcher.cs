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
		private readonly IHandlerInitializer _handlerInitializer;
		private readonly ConcurrentDictionary<Type, IHandlerInvoker> _handlerRunners;

		/// <summary>
		/// Initializes a new instance of the <see cref="Dispatcher" />.
		/// </summary>
		/// <param name="typeResolver">The resolver of CQRS handlers.</param>
		/// <param name="handlerInitializer">The handler post initializer.</param>
		/// <exception cref="ArgumentNullException"><paramref name="typeResolver" /> is null.</exception>
		public Dispatcher(ITypeResolver typeResolver, IHandlerInitializer handlerInitializer = null)
		{
			_typeResolver = typeResolver ?? throw new ArgumentNullException(nameof(typeResolver));
			_handlerInitializer = handlerInitializer;
			_handlerRunners = new ConcurrentDictionary<Type, IHandlerInvoker>();
		}

		/// <inheritdoc />
		public TResponse Fetch<TResponse>(IQuery<TResponse> query)
		{
			query = query ?? throw new ArgumentNullException(nameof(query));

			return (TResponse)_handlerRunners
				.GetOrAdd(query.GetType(), MakeFetch<TResponse>)
				.Invoke(query);
		}

		/// <inheritdoc />
		public Task<TResponse> FetchAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken)
		{
			query = query ?? throw new ArgumentNullException(nameof(query));

			return (Task<TResponse>)_handlerRunners
				.GetOrAdd(query.GetType(), MakeFetch<TResponse>)
				.InvokeAsync(query, cancellationToken);
		}

		/// <inheritdoc />
		public TResponse Process<TResponse>(ICommand<TResponse> command)
		{
			command = command ?? throw new ArgumentNullException(nameof(command));

			return (TResponse)_handlerRunners
				.GetOrAdd(command.GetType(), MakeProcess<TResponse>)
				.Invoke(command);
		}

		/// <inheritdoc />
		public Task<TResponse> ProcessAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken)
		{
			command = command ?? throw new ArgumentNullException(nameof(command));

			return (Task<TResponse>)_handlerRunners
				.GetOrAdd(command.GetType(), MakeProcess<TResponse>)
				.InvokeAsync(command, cancellationToken);
		}

		/// <inheritdoc />
		public void Publish(INotification notification)
		{
			notification = notification ?? throw new ArgumentNullException(nameof(notification));

			_handlerRunners
				.GetOrAdd(notification.GetType(), MakePublish)
				.Invoke(notification);
		}

		/// <inheritdoc />
		public Task PublishAsync(INotification notification, CancellationToken cancellationToken)
		{
			notification = notification ?? throw new ArgumentNullException(nameof(notification));

			return _handlerRunners
				.GetOrAdd(notification.GetType(), MakePublish)
				.InvokeAsync(notification, cancellationToken);
		}

		#region [Private Methods]

		private IHandlerInvoker MakeFetch<TResponse>(Type type)
		{
			return (IHandlerInvoker)CreateHandlerRunner(typeof(RequestHandlerInvoker<,>), type, typeof(TResponse));
		}

		private IHandlerInvoker MakeProcess<TResponse>(Type type)
		{
			return (IHandlerInvoker)CreateHandlerRunner(typeof(RequestHandlerInvoker<,>), type, typeof(TResponse));
		}

		private IHandlerInvoker MakePublish(Type type)
		{
			return (IHandlerInvoker)CreateHandlerRunner(typeof(NotificationHandlerInvoker<>), type);
		}

		private object CreateHandlerRunner(Type genericExecutorType, Type requestType, Type responseType)
		{
			var executorType = genericExecutorType.MakeGenericType(requestType, responseType);
			return Activator.CreateInstance(executorType, new object[] { _typeResolver, _handlerInitializer }, null);
		}

		private object CreateHandlerRunner(Type genericExecutorType, Type requestType)
		{
			var executorType = genericExecutorType.MakeGenericType(requestType);
			return Activator.CreateInstance(executorType, new object[] { _typeResolver, _handlerInitializer }, null);
		}

		#endregion
	}
}