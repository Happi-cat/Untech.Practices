using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Owin;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Owin.RequestExecutors;
using Untech.Practices.CQRS.Owin.Routing;

namespace Untech.Practices.CQRS.Owin
{
	internal class CqrsMiddleware : OwinMiddleware
	{
		private readonly IReadOnlyDictionary<RouteInfo, IRequestExecutor> _routes;
		private readonly IDispatcher _dispatcher;

		public CqrsMiddleware(OwinMiddleware next, IEnumerable<RouteInfo> routes, IDispatcher dispatcher) : base(next)
		{
			_routes = routes.ToDictionary(n => n, n => n.Executor);
			_dispatcher = dispatcher;
		}

		public override Task Invoke(IOwinContext context)
		{
			var verb = (HttpVerbs)Enum.Parse(typeof(HttpVerbs), context.Request.Method);
			var uri = context.Request.Uri;

			// get best method exec.
			IRequestExecutor executor = null;

			return executor.Handle(context, _dispatcher);
		}
	}
}