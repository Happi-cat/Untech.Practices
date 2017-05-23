using Microsoft.AspNetCore.Routing;

namespace Untech.Practices.AspNetCore.CQRS.Builder
{
	public static class RouteBuilderExtensions
	{
		public static IEndpointBuilder Route(this IRouteBuilder builder, string template)
		{
			Guard.CheckNotNull(nameof(builder), builder);
			Guard.CheckNotNull(nameof(template), template);

			return new EndpointBuilder(builder, template);
		}
	}
}
