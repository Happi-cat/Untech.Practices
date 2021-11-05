using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
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
		private delegate Task Processor(TypeResolver resolver, object input, CancellationToken token);

		private static readonly ConcurrentDictionary<Type, Processor> s_processors;

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
			s_processors = new ConcurrentDictionary<Type, Processor>();
		}

		public static void AheadOfTime<TRequest, TResponse>() where TRequest : IRequest<TResponse>
		{
			s_processors.GetOrAdd(typeof(TRequest), RequestProcessor<TRequest, TResponse>.InvokeAsync);
		}

		/// <inheritdoc />
		public Task<TResponse> FetchAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken)
		{
			query = query ?? throw new ArgumentNullException(nameof(query));

			return (Task<TResponse>)InvokeAsync(query, BuildRequestProcessor<TResponse>, cancellationToken);
		}

		/// <inheritdoc />
		public Task<TResponse> ProcessAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken)
		{
			command = command ?? throw new ArgumentNullException(nameof(command));

			return (Task<TResponse>)InvokeAsync(command, BuildRequestProcessor<TResponse>, cancellationToken);
		}

		/// <inheritdoc />
		public Task PublishAsync(IEvent @event, CancellationToken cancellationToken)
		{
			@event = @event ?? throw new ArgumentNullException(nameof(@event));

			return InvokeAsync(@event, BuildEventProcessor, cancellationToken);
		}

		#region [Private Methods]

		private Task InvokeAsync(object input, Func<Type, Processor> createFactory,
			CancellationToken cancellationToken)
		{
			return s_processors.GetOrAdd(input.GetType(), createFactory)
				.Invoke(_typeResolver, input, cancellationToken);
		}

		private static Processor BuildRequestProcessor<TResponse>(Type requestType) => Build(
			typeof(RequestProcessor<,>),
			requestType,
			typeof(TResponse));

		private static Processor BuildEventProcessor(Type requestType) => Build(
			typeof(EventProcessor<>),
			requestType);

		private static Processor Build(Type genericTypeDefinition, params Type[] typeArguments)
		{
			return BuildFactory(genericTypeDefinition.MakeGenericType(typeArguments));
		}

		private static Processor BuildFactory(Type processorType)
		{
			var methodInfo = processorType.GetMethod("InvokeAsync",
				BindingFlags.Static | BindingFlags.Public,
				null,
				new[] { typeof(TypeResolver), typeof(object), typeof(CancellationToken) },
				null
			);

			var p0 = Expression.Parameter(typeof(TypeResolver), "resolver");
			var p1 = Expression.Parameter(typeof(object), "input");
			var p2 = Expression.Parameter(typeof(CancellationToken), "token");

			return Expression
				.Lambda<Processor>(Expression.Call(methodInfo, p0, p1, p2), p0, p1, p2)
				.Compile();
		}

		#endregion
	}
}