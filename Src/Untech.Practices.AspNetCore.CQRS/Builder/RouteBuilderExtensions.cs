using Microsoft.AspNetCore.Routing;

namespace Untech.Practices.AspNetCore.CQRS.Builder
{
	public static class RouteBuilderExtensions
	{
		public static IEndpointBuilder Route(this IRouteBuilder builder, string template)
		{
			return new EndpointBuilder(builder, template);
		}
	}
}
