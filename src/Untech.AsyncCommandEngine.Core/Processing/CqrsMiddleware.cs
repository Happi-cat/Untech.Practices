using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Untech.Practices.CQRS;

namespace Untech.AsyncCommandEngine.Processing
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
			s_executeNotificationMethodInfo = GetMethod(nameof(ExecuteNotificationAsync));

			MethodInfo GetMethod(string name)
			{
				return myType.GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic);
			}
		}

		public CqrsMiddleware(ICqrsStrategy strategy)
		{
			_strategy = strategy;
		}

		public Task InvokeAsync(Context context, RequestProcessorCallback next)
		{
			context.Aborted.ThrowIfCancellationRequested();

			return s_executors
				.GetOrAdd(context.RequestName, BuildExecutor)
				.Invoke(this, context);
		}

		private ExecutorCallback BuildExecutor(string requestName)
		{
			var requestType = _strategy.FindRequestType(requestName);

			return BuildExecutor(requestType);
		}

		private ExecutorCallback BuildExecutor(Type requestType)
		{
			var callbacks = requestType
				.GetTypeInfo().ImplementedInterfaces
				.Select(TryBuildCallback)
				.Where(callback => callback != null)
				.ToList();

			if (callbacks.Count > 1)
				throw new InvalidOperationException($"Request type {requestType} has multiple interfaces of type ICommand<> and INotification)");
			if (callbacks.Count == 0)
				throw new InvalidOperationException($"Request type {requestType} doesn't implements ICommand<> or INotification");

			return callbacks[0];

			ExecutorCallback TryBuildCallback(Type ifType)
			{
				if (!ifType.IsGenericType) return null;

				var genericTypeDef = ifType.GetGenericTypeDefinition();

				if (genericTypeDef == typeof(ICommand<>))
				{
					var resultType = ifType.GetGenericArguments()[0];

					var method = s_executeCommandMethodInfo.MakeGenericMethod(requestType, resultType);

					return BuildCallback(method);
				}

				if (genericTypeDef == typeof(INotification))
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
			var dispatcher = middleware._strategy.GetDispatcher(context);
			var command = middleware._strategy.MaterializeRequest(context.Request);

			return dispatcher.ProcessAsync((TRequest)command, context.Aborted);
		}

		private static Task ExecuteNotificationAsync<TNotification>(CqrsMiddleware middleware, Context context)
			where TNotification: INotification
		{
			var dispatcher = middleware._strategy.GetDispatcher(context);
			var notification = middleware._strategy.MaterializeRequest(context.Request);

			return dispatcher.PublishAsync((TNotification)notification, context.Aborted);
		}
	}
}