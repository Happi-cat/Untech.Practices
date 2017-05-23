using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.DependencyInjection;

namespace Untech.Practices.AspNetCore.CQRS.Builder
{
	internal class EndpointBuilder : IEndpointBuilder
	{
		private readonly IRouteBuilder _builder;
		private readonly string _template;

		public EndpointBuilder(IRouteBuilder routeBuilder, string template)
		{
			Guard.CheckNotNull(nameof(routeBuilder), routeBuilder);
			Guard.CheckNotNull(nameof(template), template);

			_builder = routeBuilder;
			_template = template;
		}

		public IEndpointBuilder MapVerb<TIn, TOut>(string verb, CqrsHandler<TIn, TOut> handler)
		{
			Guard.CheckNotNullOrEmpty(nameof(verb), verb);
			Guard.CheckNotNull(nameof(handler), handler);

			var route = new Route(handler, _template,
				defaults: null,
				constraints: new RouteValueDictionary(new { httpMethod = new HttpMethodRouteConstraint(verb) }),
				dataTokens: null,
				inlineConstraintResolver: GetConstraintResolver(_builder));

			_builder.Routes.Add(route);
			return this;
		}

		private static IInlineConstraintResolver GetConstraintResolver(IRouteBuilder builder)
		{
			return builder.ServiceProvider.GetRequiredService<IInlineConstraintResolver>();
		}
	}
}
