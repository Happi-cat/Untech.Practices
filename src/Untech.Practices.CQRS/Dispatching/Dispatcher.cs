using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Untech.Practices.CQRS.Dispatching.Processors;

namespace Untech.Practices.CQRS.Dispatching
{
	/// <summary>
	///     Represents class that implements <see cref="IDispatcher" /> interface
	///     and dispatches incoming CQRS request to appropriate handlers.
	/// </summary>
	public sealed class Dispatcher : IDispatcher
	{
		private readonly ConcurrentDictionary<Type, IProcessor> _processors;
		private readonly ITypeResolver _typeResolver;

		/// <summary>
		///     Initializes a new instance of the <see cref="Dispatcher" />.
		/// </summary>
		/// <param name="typeResolver">The resolver of CQRS handlers.</param>
		/// <exception cref="ArgumentNullException"><paramref name="typeResolver" /> is null.</exception>
		public Dispatcher(ITypeResolver typeResolver)
		{
			_typeResolver = typeResolver ?? throw new ArgumentNullException(nameof(typeResolver));
			_processors = new ConcurrentDictionary<Type, IProcessor>();
		}

		/// <inheritdoc />
		public Task<TResponse> FetchAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken)
		{
			query = query ?? throw new ArgumentNullException(nameof(query));

			return (Task<TResponse>)_processors
				.GetOrAdd(query.GetType(), CreateForFetch<TResponse>)
				.InvokeAsync(query, cancellationToken);
		}

		/// <inheritdoc />
		public Task<TResponse> ProcessAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken)
		{
			command = command ?? throw new ArgumentNullException(nameof(command));

			return (Task<TResponse>)_processors
				.GetOrAdd(command.GetType(), CreateForProcess<TResponse>)
				.InvokeAsync(command, cancellationToken);
		}

		/// <inheritdoc />
		public Task PublishAsync(IEvent @event, CancellationToken cancellationToken)
		{
			@event = @event ?? throw new ArgumentNullException(nameof(@event));

			return _processors
				.GetOrAdd(@event.GetType(), CreateForPublish)
				.InvokeAsync(@event, cancellationToken);
		}

		#region [Private Methods]

		private IProcessor CreateForFetch<TResponse>(Type requestType)
		{
			return (IProcessor)CreateProcessor(
				typeof(RequestProcessor<,>),
				requestType,
				typeof(TResponse));
		}

		private IProcessor CreateForProcess<TResponse>(Type requestType)
		{
			return (IProcessor)CreateProcessor(
				typeof(RequestProcessor<,>),
				requestType,
				typeof(TResponse));
		}

		private IProcessor CreateForPublish(Type requestType)
		{
			return (IProcessor)CreateProcessor(
				typeof(EventProcessor<>),
				requestType);
		}

		private object CreateProcessor(Type genericTypeDefinition, params Type[] typeArguments)
		{
			Type genericType = genericTypeDefinition.MakeGenericType(typeArguments);
			return Activator.CreateInstance(genericType, new object[] { _typeResolver }, null);
		}

		#endregion
	}
}