using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Microsoft.AspNetCore.Builder
{
	public static class CqrsApplicationBuilderExtensions
	{
		public static IApplicationBuilder UseCqrs(this IApplicationBuilder app, Action<IRouteBuilder> configureRoutes)
		{
			app = app ?? throw new ArgumentNullException(nameof(app));
			configureRoutes = configureRoutes ?? throw new ArgumentNullException(nameof(configureRoutes));

			var router = new RouteBuilder(app);
			configureRoutes(router);
			return app.UseRouter(router.Build());
		}

		public static IApplicationBuilder UseCqrs(this IApplicationBuilder app, RequestDelegate defaultHandler, Action<IRouteBuilder> configureRoutes)
		{
			app = app ?? throw new ArgumentNullException(nameof(app));
			configureRoutes = configureRoutes ?? throw new ArgumentNullException(nameof(configureRoutes));

			var router = new RouteBuilder(app, new RouteHandler(defaultHandler));
			configureRoutes(router);
			return app.UseRouter(router.Build());
		}
	}
}
