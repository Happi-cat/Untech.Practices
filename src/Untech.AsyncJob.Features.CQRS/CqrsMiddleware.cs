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
	internal class CqrsMiddleware : IRequestProcessorMiddleware
	{
		private static readonly MethodInfo s_executeCommandMethodInfo;
		private static readonly MethodInfo s_executeNotificationMethodInfo;

		private static readonly ConcurrentDictionary<string, ExecutorCallback> s_executors;

		private delegate Task ExecutorCallback(CqrsMiddleware middleware, Context context);

		private readonly ICqrsStrategy _strategy;

		static CqrsMiddleware()
		{
			s_executors = new ConcurrentDictionary<string, ExecutorCallback>();

			var myType = typeof(CqrsMiddleware);
			s_executeCommandMethodInfo = GetMethod(nameof(ExecuteCommandAsync));
			s_executeNotificationMethodInfo = GetMethod(nameof(ExecuteEventAsync));

			MethodInfo GetMethod(string name)
			{
				return myType.GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic);
			}
		}

		public CqrsMiddleware(ICqrsStrategy strategy)
		{
			_strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
		}

		public Task InvokeAsync(Context context, RequestProcessorCallback next)
		{
			if (string.IsNullOrEmpty(context.RequestName)) throw new ArgumentException(nameof(context.RequestName));

			context.Aborted.ThrowIfCancellationRequested();

			return s_executors
				.GetOrAdd(context.RequestName, BuildExecutor)
				.Invoke(this, context);
		}

		private ExecutorCallback BuildExecutor(string requestName)
		{
			var requestType = _strategy.FindRequestType(requestName) ?? throw RequestTypeNotFoundError(requestName);

			return BuildExecutor(requestType);
		}

		private ExecutorCallback BuildExecutor(Type requestType)
		{
			var callbacks = requestType
				.GetTypeInfo().ImplementedInterfaces
				.Select(TryBuildCallback)
				.Where(callback => callback != null)
				.ToList();

			if (callbacks.Count > 1) throw RequestTypeHasMultipleInterfacesError(requestType);
			if (callbacks.Count == 0) throw RequestTypeHasNoInterfacesError(requestType);

			return callbacks[0];

			ExecutorCallback TryBuildCallback(Type implementedInterfaceType)
			{
				if (!implementedInterfaceType.IsGenericType) return null;

				var genericTypeDefinition = implementedInterfaceType.GetGenericTypeDefinition();

				if (genericTypeDefinition == typeof(ICommand<>))
				{
					var resultType = implementedInterfaceType.GetGenericArguments()[0];

					var method = s_executeCommandMethodInfo.MakeGenericMethod(requestType, resultType);

					return BuildCallback(method);
				}

				if (genericTypeDefinition == typeof(IEvent))
				{
					var method = s_executeNotificationMethodInfo.MakeGenericMethod(requestType);

					return BuildCallback(method);
				}

				return null;
			}

			ExecutorCallback BuildCallback(MethodInfo methodInfo)
			{
				var p0 = Expression.Parameter(typeof(CqrsMiddleware), "middleware");
				var p1 = Expression.Parameter(typeof(Context), "context");

				var lambda = Expression.Lambda<ExecutorCallback>(
					Expression.Call(methodInfo, p0, p1),
					p0, p1
				);

				return lambda.Compile();
			}
		}

		private static Task ExecuteCommandAsync<TRequest, TResult>(CqrsMiddleware middleware, Context context)
			where TRequest: ICommand<TResult>
		{
			var dispatcher = middleware._strategy.GetDispatcher(context) ?? throw NoDispatcherError();
			var command = context.Request.GetBody(typeof(TRequest)) ?? throw NoRequestError();

			return dispatcher.ProcessAsync((TRequest)command, context.Aborted);
		}

		private static Task ExecuteEventAsync<TEvent>(CqrsMiddleware middleware, Context context)
			where TEvent: IEvent
		{
			var dispatcher = middleware._strategy.GetDispatcher(context) ?? throw NoDispatcherError();
			var notification = context.Request.GetBody(typeof(TEvent)) ?? throw NoRequestError();

			return dispatcher.PublishAsync((TEvent)notification, context.Aborted);
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

		private static Exception NoRequestError()
		{
			return new InvalidOperationException("Request wasn't parsed and null was returned.");
		}
	}
}
