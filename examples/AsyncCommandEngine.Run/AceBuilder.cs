using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Hosting;
using Untech.AsyncCommmandEngine.Abstractions;

namespace AsyncCommandEngine.Run
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
		public IHostedService Build()
		{
			throw new NotImplementedException();
		}

		internal AceProcessor BuildService()
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