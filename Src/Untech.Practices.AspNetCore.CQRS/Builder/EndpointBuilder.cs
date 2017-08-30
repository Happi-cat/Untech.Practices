using System;
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
			routeBuilder = routeBuilder ?? throw new ArgumentNullException(nameof(routeBuilder));
			template = template ?? throw new ArgumentNullException(nameof(template));


			_builder = routeBuilder;
			_template = template;
		}

		public IEndpointBuilder MapVerb<TIn, TOut>(string verb, CqrsHandler<TIn, TOut> handler)
		{
			verb = verb ?? throw new ArgumentNullException(nameof(verb));
			handler = handler ?? throw new ArgumentNullException(nameof(handler));


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
