using Untech.Practices.CQRS.Owin.RequestExecutors;
using Untech.Practices.CQRS.Owin.RequestMappers;

namespace Untech.Practices.CQRS.Owin.Routing
{
	public static class RouteBuilderExtensions
	{
		public static IRouteBuilder UseQuery<TIn, TOut>(this IRouteBuilder routeBuilder, IRequestMapper<TIn> mapper = null)
			where TIn : IQuery<TOut>
		{
			return routeBuilder.Use(HttpVerbs.Get, new QueryRequestExecutor<TIn, TOut>(mapper ?? new DefaultRequestMapper<TIn>()));
		}

		public static IRouteBuilder UseCommand<TIn, TOut>(this IRouteBuilder routeBuilder, HttpVerbs verbs, IRequestMapper<TIn> mapper = null)
			where TIn : ICommand<TOut>
		{
			return routeBuilder.Use(verbs, new CommandRequestExecutor<TIn, TOut>(mapper ?? new DefaultRequestMapper<TIn>()));
		}

		public static IRouteBuilder UseNotification<TIn>(this IRouteBuilder routeBuilder, IRequestMapper<TIn> mapper = null)
			where TIn : INotification
		{
			return routeBuilder.Use(HttpVerbs.Post, new NotificationRequestExecutor<TIn>(mapper ?? new DefaultRequestMapper<TIn>()));
		}
	}
}