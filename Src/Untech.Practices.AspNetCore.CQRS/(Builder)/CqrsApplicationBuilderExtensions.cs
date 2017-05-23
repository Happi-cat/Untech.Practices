using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Untech.Practices;

namespace Microsoft.AspNetCore.Builder
{
	public static class CqrsApplicationBuilderExtensions
	{
		public static IApplicationBuilder UseCqrs(this IApplicationBuilder app, Action<IRouteBuilder> configureRoutes)
		{
			Guard.CheckNotNull(nameof(app), app);
			Guard.CheckNotNull(nameof(configureRoutes), configureRoutes);

			var router = new RouteBuilder(app);
			configureRoutes(router);
			return app.UseRouter(router.Build());
		}

		public static IApplicationBuilder UseCqrs(this IApplicationBuilder app, RequestDelegate defaultHandler, Action<IRouteBuilder> configureRoutes)
		{
			Guard.CheckNotNull(nameof(app), app);
			Guard.CheckNotNull(nameof(configureRoutes), configureRoutes);

			var router = new RouteBuilder(app, new RouteHandler(defaultHandler));
			configureRoutes(router);
			return app.UseRouter(router.Build());
		}
	}
}
