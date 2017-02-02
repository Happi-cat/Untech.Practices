using System.Collections.Generic;
using Untech.Practices.CQRS.Owin.RequestExecutors;

namespace Untech.Practices.CQRS.Owin.Routing
{
	public interface IRouteTableBuilder
	{
		IEnumerable<RouteInfo> GetRoutes();

		IEnumerable<RouteInfo> GetRoutes(string prefix);
	}

	public interface IRouteBuilder : IRouteTableBuilder
	{
		IRouteBuilder Use(HttpVerbs verbs, IRequestExecutor requestExecutor);
	}
}