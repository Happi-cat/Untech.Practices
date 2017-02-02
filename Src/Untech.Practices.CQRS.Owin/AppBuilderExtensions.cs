using Owin;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Owin.Routing;

namespace Untech.Practices.CQRS.Owin
{
	public static class AppBuilderExtensions
	{
		public static IAppBuilder UseCqrsApi(this IAppBuilder appBuilder, IRouteBuilder routeBuilder, IDispatcher dispatcher)
		{
			appBuilder.Use(typeof(CqrsMiddleware), routeBuilder.GetRoutes(), dispatcher);
			return appBuilder;
		}
	}
}
