using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq.Expressions;
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
		private delegate IProcessor ProcessorFactory(TypeResolver resolver);

		private static readonly ConcurrentDictionary<Type, ProcessorFactory> s_processors;

		private readonly TypeResolver _typeResolver;

		/// <summary>
		///     Initializes a new instance of the <see cref="Dispatcher" />.
		/// </summary>
		/// <param name="typeResolver">The resolver of CQRS handlers.</param>
		/// <exception cref="ArgumentNullException"><paramref name="typeResolver" /> is null.</exception>
		public Dispatcher(IServiceProvider typeResolver)
		{
			_typeResolver = new TypeResolver(typeResolver ?? throw new ArgumentNullException(nameof(typeResolver)));
		}

		static Dispatcher()
		{
			s_processors = new ConcurrentDictionary<Type, ProcessorFactory>();
		}

		/// <inheritdoc />
		public Task<TResponse> FetchAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken)
		{
			query = query ?? throw new ArgumentNullException(nameof(query));

			return (Task<TResponse>)InvokeAsync(query, BuildRequestProcessorFactory<TResponse>, cancellationToken);
		}

		/// <inheritdoc />
		public Task<TResponse> ProcessAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken)
		{
			command = command ?? throw new ArgumentNullException(nameof(command));

			return (Task<TResponse>)InvokeAsync(command, BuildRequestProcessorFactory<TResponse>, cancellationToken);
		}

		/// <inheritdoc />
		public Task PublishAsync(IEvent @event, CancellationToken cancellationToken)
		{
			@event = @event ?? throw new ArgumentNullException(nameof(@event));

			return InvokeAsync(@event, BuildEventProcessorFactory, cancellationToken);
		}

		#region [Private Methods]

		private Task InvokeAsync(object input, Func<Type, ProcessorFactory> createFactory,
			CancellationToken cancellationToken)
		{
			return s_processors.GetOrAdd(input.GetType(), createFactory)
				.Invoke(_typeResolver)
				.InvokeAsync(input, cancellationToken);
		}

		private static ProcessorFactory BuildRequestProcessorFactory<TResponse>(Type requestType) => BuildFactory(
			typeof(RequestProcessor<,>),
			requestType,
			typeof(TResponse));

		private static ProcessorFactory BuildEventProcessorFactory(Type requestType) => BuildFactory(
			typeof(EventProcessor<>),
			requestType);

		private static ProcessorFactory BuildFactory(Type genericTypeDefinition, params Type[] typeArguments)
		{
			return BuildFactory(genericTypeDefinition.MakeGenericType(typeArguments));
		}

		private static ProcessorFactory BuildFactory(Type processorType)
		{
			var ctor = processorType.GetConstructor(new [] { typeof(TypeResolver) });

			var p = Expression.Parameter(typeof(TypeResolver), "resolver");

			return Expression
				.Lambda<ProcessorFactory>(Expression.New(ctor, p), p)
				.Compile();
		}

		#endregion
	}
}