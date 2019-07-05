using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AsyncJob.Run.Commands;
using Microsoft.Extensions.Logging;
using Untech.AsyncJob;
using Untech.AsyncJob.Features.CQRS;
using Untech.AsyncJob.Processing;
using Untech.Practices.CQRS.Dispatching;

namespace AsyncJob.Run
{
	internal class CqrsStrategy : ICqrsStrategy, ITypeResolver
	{
		private readonly ILogger _logger;

		private readonly IReadOnlyList<Type> _types;

		public CqrsStrategy(ILogger logger)
		{
			var demoType = typeof(DemoCommandBase);
			_logger = logger;
			_types = typeof(CqrsStrategy).Assembly.GetTypes()
				.Where(t => demoType.IsAssignableFrom(t))
				.ToList();
		}

		public T ResolveOne<T>() where T : class
		{
			return new DemoHandlers(_logger) as T;
		}

		public IEnumerable<T> ResolveMany<T>() where T : class
		{
			return Enumerable.Empty<T>();
		}

		public Type FindRequestType(string requestName)
		{
			return _types.FirstOrDefault(t => t.FullName == requestName)
				?? throw new ArgumentException($"There is no {requestName}");
		}

		public IDispatcher GetDispatcher(Context context)
		{
			return new Dispatcher(this);
		}
	}
}
