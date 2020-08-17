using System;
using Untech.AsyncJob.Builder.Registrars;
using Untech.AsyncJob.Processing;

namespace Untech.AsyncJob.Builder.Factories
{
	internal class RequestProcessorFactory : IServiceFactory<IRequestProcessor>
	{
		private readonly MultiRegistrar<IRequestProcessorMiddleware> _middleware;
		private readonly SingleRegistrar<IRequestProcessor> _endware;

		public RequestProcessorFactory(MultiRegistrar<IRequestProcessorMiddleware> middleware,
			SingleRegistrar<IRequestProcessor> endware)
		{
			_middleware = middleware;
			_endware = endware;
		}

		public IRequestProcessor Create(IServiceProvider serviceProvider)
		{
			var middleware = _middleware.CreateAll<IRequestProcessorMiddleware>(serviceProvider);
			var endware = _endware.CreateOne<IRequestProcessor>(serviceProvider);

			if (middleware.Count == 0) return endware ?? throw new InvalidOperationException("No registered processor");
			if (endware == null) return new RequestProcessor(middleware);
			return new RequestProcessor(middleware, endware);
		}
	}
}