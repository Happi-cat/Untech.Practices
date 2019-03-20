using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.AsyncCommandEngine
{
	public class AceBuilder
	{
		private readonly List<IAceProcessorMiddleware> _middlewares = new List<IAceProcessorMiddleware>();

		public AceBuilder UseTransport()
		{
			throw new NotImplementedException();
		}

		public AceBuilder Use(Func<IAceProcessorMiddleware> creator)
		{
			_middlewares.Add(creator());
			return this;
		}

		public AceProcessor BuildService()
		{
			var predefinedMiddlewares = new IAceProcessorMiddleware[]
			{

				new EnsureIsNotAbortedMiddleware(),
				new CqrsMiddleware(null),
			};

			return new AceProcessor(_middlewares.Concat(predefinedMiddlewares));
		}
	}
}