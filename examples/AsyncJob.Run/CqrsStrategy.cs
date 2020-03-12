using System;
using System.Collections.Generic;
using System.Linq;
using AsyncJob.Run.Commands;
using Microsoft.Extensions.Logging;
using Untech.AsyncJob;
using Untech.AsyncJob.Features.CQRS;
using Untech.Practices.CQRS.Dispatching;

namespace AsyncJob.Run
{
	internal class CqrsStrategy : ICqrsStrategy, IServiceProvider
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

		public object GetService(Type serviceType)
		{
			if (serviceType.IsAssignableFrom(typeof(DemoHandlers)))
				return new DemoHandlers(_logger);
			throw new InvalidOperationException();
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
