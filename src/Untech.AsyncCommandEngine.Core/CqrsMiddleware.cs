using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Untech.Practices.CQRS;
using Untech.Practices.CQRS.Dispatching;

namespace Untech.AsyncCommandEngine
{
	public interface IRequestTypeFinder
	{
		TypeInfo Find(string typeName);
	}

	public interface IRequestMaterializer
	{
		object Materialize(Request request);
	}
	internal class CqrsMiddleware : IRequestProcessorMiddleware
	{
		private static readonly MethodInfo s_executeCommandMethodInfo;
		private static readonly MethodInfo s_executeNotificationMethodInfo;

		private static readonly ConcurrentDictionary<string, ExecutorCallback> s_executors;

		private delegate Task ExecutorCallback(CqrsMiddleware middleware, Context context);

		private readonly IRequestTypeFinder _requestTypeFinder;
		private readonly IRequestMaterializer _materializer;
		private readonly Func<Context, IDispatcher> _dispatcherSelector;

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

		public CqrsMiddleware(IRequestTypeFinder requestTypeFinder,
			IRequestMaterializer materializer,
			Func<Context, IDispatcher> dispatcherSelector)
		{
			_requestTypeFinder = requestTypeFinder;
			_materializer = materializer;
			_dispatcherSelector = dispatcherSelector;
		}

		public Task ExecuteAsync(Context context, RequestProcessorCallback next)
		{
			var requestType = _requestTypeFinder.Find(context.RequestName);

			return s_executors
				.GetOrAdd(context.RequestName, name => BuildExecutor(requestType))
				.Invoke(this, context);
		}

		private ExecutorCallback BuildExecutor(Type requestType)
		{
			var callbacks = new List<ExecutorCallback>();

			foreach (var implementedInterface in requestType.GetTypeInfo().ImplementedInterfaces)
			{
				var callback = BuildCallbackOrNull(implementedInterface);
				if (callback != null)
					callbacks.Add(callback);
			}

			if (callbacks.Count > 1)
				throw new InvalidOperationException($"Request type {requestType} has multiple interfaces of type ICommand<> and INotification)");
			if (callbacks.Count == 0)
				throw new InvalidOperationException($"Request type {requestType} doesn't implements ICommand<> or INotification");

			return callbacks[0];

			ExecutorCallback BuildCallbackOrNull(Type ifType)
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
					Expression.Call(methodInfo, p0, p1)
					,
					p0, p1
				);

				return lambda.Compile();
			}
		}

		private static Task ExecuteCommandAsync<TRequest, TResult>(CqrsMiddleware middleware, Context context)
			where TRequest: ICommand<TResult>
		{
			Debugger.Break();

			var dispatcher = middleware._dispatcherSelector(context);
			var command = middleware._materializer.Materialize(context.Request);

			return dispatcher.ProcessAsync((TRequest)command, context.Aborted);
		}

		private static Task ExecuteNotificationAsync<TNotification>(CqrsMiddleware middleware, Context context)
			where TNotification: INotification
		{
			var dispatcher = middleware._dispatcherSelector(context);
			var notification = middleware._materializer.Materialize(context.Request);

			return dispatcher.PublishAsync((TNotification)notification, context.Aborted);
		}
	}
}