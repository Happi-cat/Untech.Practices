using System;
using AsyncJob.Run.Commands;
using Microsoft.Extensions.Logging;

namespace AsyncJob.Run
{
	internal class DemoServiceProvider : IServiceProvider
	{
		private readonly ILogger _logger;

		public DemoServiceProvider(ILogger logger)
		{
			_logger = logger;
		}

		public object GetService(Type serviceType)
		{
			if (serviceType.IsAssignableFrom(typeof(DemoHandlers)))
				return new DemoHandlers(_logger);
			throw new InvalidOperationException();
		}
	}
}