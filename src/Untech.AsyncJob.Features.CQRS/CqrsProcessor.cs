using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Untech.AsyncJob.Processing;
using Untech.Practices.CQRS;

namespace Untech.AsyncJob.Features.CQRS
{
	public sealed class CqrsProcessor : IRequestProcessor
	{
		private static readonly MethodInfo s_executeCommandMethodInfo;
		private static readonly MethodInfo s_executeNotificationMethodInfo;

		private static readonly ConcurrentDictionary<string, ExecutorCallback> s_executors;

		private delegate Task ExecutorCallback(CqrsProcessor processor, Context context);

		private readonly ICqrsStrategy _strategy;

		static CqrsProcessor()
		{
			s_executors = new ConcurrentDictionary<string, ExecutorCallback>();

			var myType = typeof(CqrsProcessor);
			s_executeCommandMethodInfo = GetMethod(nameof(ExecuteCommandAsync));
			s_executeNotificationMethodInfo = GetMethod(nameof(ExecuteEventAsync));

			MethodInfo GetMethod(string name)
			{
				return myType.GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic);
			}
		}

		public CqrsProcessor(ICqrsStrategy strategy)
		{
			_strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
		}

		public Task InvokeAsync(Context context)
		{
			if (string.IsNullOrEmpty(context.RequestName))
				throw new ArgumentException(nameof(context.RequestName));

			context.Aborted.ThrowIfCancellationRequested();

			return s_executors
				.GetOrAdd(context.RequestName, BuildExecutor)
				.Invoke(this, context);
		}

		private ExecutorCallback BuildExecutor(string requestName)
		{
			var requestType = _strategy.GetRequestTypeFinder().FindRequestType(requestName)
				?? throw RequestTypeNotFoundError(requestName);

			return BuildExecutor(requestType);
		}

		private static ExecutorCallback BuildExecutor(Type requestType)
		{
			var callbacks = requestType
				.GetTypeInfo().ImplementedInterfaces
				.Select(TryBuildCallback)
				.Where(callback => callback != null)
				.ToList();

			if (callbacks.Count > 1)
				throw RequestTypeHasMultipleInterfacesError(requestType);
			if (callbacks.Count == 0)
				throw RequestTypeHasNoInterfacesError(requestType);

			return callbacks[0];

			ExecutorCallback TryBuildCallback(Type implementedInterfaceType)
			{
				if (implementedInterfaceType == typeof(IEvent))
				{
					var method = s_executeNotificationMethodInfo.MakeGenericMethod(requestType);

					return BuildCallback(method);
				}

				if (implementedInterfaceType.IsGenericType)
				{
					var genericTypeDefinition = implementedInterfaceType.GetGenericTypeDefinition();

					if (genericTypeDefinition == typeof(ICommand<>))
					{
						var resultType = implementedInterfaceType.GetGenericArguments()[0];

						var method = s_executeCommandMethodInfo.MakeGenericMethod(requestType, resultType);

						return BuildCallback(method);
					}
				}

				return null;
			}

			ExecutorCallback BuildCallback(MethodInfo methodInfo)
			{
				var p0 = Expression.Parameter(typeof(CqrsProcessor), "middleware");
				var p1 = Expression.Parameter(typeof(Context), "context");

				var lambda = Expression.Lambda<ExecutorCallback>(
					Expression.Call(methodInfo, p0, p1),
					p0, p1
				);

				return lambda.Compile();
			}
		}

		private static Task ExecuteCommandAsync<TRequest, TResult>(CqrsProcessor processor, Context context)
			where TRequest : ICommand<TResult>
		{
			var dispatcher = processor._strategy.GetDispatcher(context) ?? throw NoDispatcherError();
			var command = GetRequestOrThrow(processor, context, typeof(TRequest));

			return dispatcher.ProcessAsync((TRequest)command, context.Aborted);
		}

		private static Task ExecuteEventAsync<TEvent>(CqrsProcessor processor, Context context)
			where TEvent : IEvent
		{
			var dispatcher = processor._strategy.GetDispatcher(context) ?? throw NoDispatcherError();
			var notification = GetRequestOrThrow(processor, context, typeof(TEvent));

			return dispatcher.PublishAsync((TEvent)notification, context.Aborted);
		}

		private static object GetRequestOrThrow(CqrsProcessor processor, Context context, Type type)
		{
			var formatter = processor._strategy.GetRequestFormatter(context) ?? throw NoRequestFormatterError();
			return formatter.Deserialize(context.Request.Content, type) ?? throw NoRequestError();
		}

		private static Exception RequestTypeNotFoundError(string requestName)
		{
			return new InvalidOperationException($"Request type {requestName} was not found.");
		}

		private static Exception RequestTypeHasNoInterfacesError(Type requestType)
		{
			return new InvalidOperationException($"Request type {requestType} doesn't implements ICommand<> or INotification.");
		}

		private static Exception RequestTypeHasMultipleInterfacesError(Type requestType)
		{
			return new InvalidOperationException($"Request type {requestType} has multiple interfaces of type ICommand<> and INotification).");
		}

		private static Exception NoDispatcherError()
		{
			return new InvalidOperationException("Dispatcher is missing.");
		}

		private static Exception NoRequestFormatterError()
		{
			return new InvalidOperationException("Request formatter is missing.");
		}

		private static Exception NoRequestError()
		{
			return new InvalidOperationException("Request wasn't parsed and null was returned.");
		}
	}
}
