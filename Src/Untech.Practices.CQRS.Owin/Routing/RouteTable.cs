using System.Collections.Generic;
using System.Linq;
using Untech.Practices.CQRS.Owin.RequestExecutors;

namespace Untech.Practices.CQRS.Owin.Routing
{
	public class RouteTable : IRouteTableBuilder
	{
		private readonly List<IRouteTableBuilder> _routeBuilders = new List<IRouteTableBuilder>();

		public IRouteBuilder Route(string urlTemplate)
		{
			var builder = new RouteBuilder(urlTemplate);
			_routeBuilders.Add(builder);
			return builder;
		}

		public RouteTable Use(IRouteTableBuilder childRouteTable)
		{
			_routeBuilders.Add(childRouteTable);
			return this;
		}

		public RouteTable Use(string prefix, IRouteTableBuilder childRouteTable)
		{
			_routeBuilders.Add(new PrefixedRouteTableBuilder(prefix, childRouteTable));
			return this;
		}

		public IEnumerable<RouteInfo> GetRoutes() => _routeBuilders
			.SelectMany(n => n.GetRoutes());

		public IEnumerable<RouteInfo> GetRoutes(string prefix) => _routeBuilders
			.SelectMany(n => n.GetRoutes(prefix));

		#region [Nested Classes]

		private class RouteBuilder : IRouteBuilder
		{
			private readonly Dictionary<HttpVerbs, IRequestExecutor> _executors = new Dictionary<HttpVerbs, IRequestExecutor>();
			private readonly string _urlTemplate;

			public RouteBuilder(string urlTemplate)
			{
				_urlTemplate = urlTemplate;
			}

			public IRouteBuilder Use(HttpVerbs verbs, IRequestExecutor requestExecutor)
			{
				_executors.Add(verbs, requestExecutor);
				return this;
			}

			public IEnumerable<RouteInfo> GetRoutes() => _executors
				.Select(n => new RouteInfo
				{
					UrlTemplate = _urlTemplate,
					Method = n.Key.ToString().ToUpper(),
					Executor = n.Value
				});

			public IEnumerable<RouteInfo> GetRoutes(string prefix) => _executors
				.Select(n => new RouteInfo
				{
					UrlTemplate = prefix + _urlTemplate,
					Method = n.Key.ToString().ToUpper(),
					Executor = n.Value
				});
		}

		private class PrefixedRouteTableBuilder : IRouteTableBuilder
		{
			private readonly string _prefix;
			private readonly IRouteTableBuilder _innerBuilder;

			public PrefixedRouteTableBuilder(string prefix, IRouteTableBuilder innerBuilder)
			{
				_prefix = prefix;
				_innerBuilder = innerBuilder;
			}

			public IEnumerable<RouteInfo> GetRoutes() => _innerBuilder
				.GetRoutes(_prefix);

			public IEnumerable<RouteInfo> GetRoutes(string prefix) => _innerBuilder
				.GetRoutes(prefix + _prefix);
		}

		#endregion
	}
}