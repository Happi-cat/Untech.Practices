using System;
using System.Collections.Generic;
using System.Linq;
using Untech.AsyncCommandEngine.Processing;
using Untech.Practices.CQRS.Dispatching;

namespace Untech.AsyncCommandEngine
{
	public class AceBuilder
	{
		private readonly List<IRequestProcessorMiddleware> _middlewares = new List<IRequestProcessorMiddleware>();

		public AceBuilder UseTransport()
		{
			throw new NotImplementedException();
		}

		public AceBuilder Use(Func<IRequestProcessorMiddleware> creator)
		{
			_middlewares.Add(creator());
			return this;
		}

		public IRequestProcessor BuildService(ICqrsStrategy strategy)
		{
			var predefinedMiddleware = new IRequestProcessorMiddleware[]
			{
				new CqrsMiddleware(strategy),
			};

			return new RequestProcessor(_middlewares.Concat(predefinedMiddleware));
		}
	}
}