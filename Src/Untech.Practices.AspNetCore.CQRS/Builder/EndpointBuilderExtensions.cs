using Microsoft.AspNetCore.Http;
using Untech.Practices.CQRS;

namespace Untech.Practices.AspNetCore.CQRS.Builder
{
	public static class EndpointBuilderExtensions
	{
		public static IEndpointBuilder MapGet<TIn, TOut>(this IEndpointBuilder builder, Query<TIn, TOut> handler)
			where TIn : IQuery<TOut>
		{
			return builder.MapVerb(HttpMethods.Get, handler);
		}

		public static IEndpointBuilder MapGet<TIn, TOut>(this IEndpointBuilder builder, QueryAsync<TIn, TOut> handler)
			where TIn : IQuery<TOut>
		{
			return builder.MapVerb(HttpMethods.Get, handler);
		}

		public static IEndpointBuilder MapPost<TIn>(this IEndpointBuilder builder, Notification<TIn> handler)
			where TIn : INotification
		{
			return builder.MapVerb(HttpMethods.Post, handler);
		}

		public static IEndpointBuilder MapPost<TIn>(this IEndpointBuilder builder, NotificationAsync<TIn> handler)
			where TIn : INotification
		{
			return builder.MapVerb(HttpMethods.Post, handler);
		}

		public static IEndpointBuilder MapVerb<TIn, TOut>(this IEndpointBuilder builder, string verb, Command<TIn, TOut> handler)
			where TIn : ICommand<TOut>
		{
			return builder.MapVerb(verb, handler);
		}

		public static IEndpointBuilder MapVerb<TIn, TOut>(this IEndpointBuilder builder, string verb, CommandAsync<TIn, TOut> handler)
			where TIn : ICommand<TOut>
		{
			return builder.MapVerb(verb, handler);
		}
	}
}
