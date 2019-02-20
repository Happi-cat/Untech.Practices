using System;

namespace Untech.Practices.CQRS.Handlers
{
	public static class Handlers
	{
		/// <summary>
		///     Returns dummy query handler.
		/// </summary>
		/// <typeparam name="TIn">The type of query to be handled.</typeparam>
		/// <typeparam name="TOut">The type of result from the handler.</typeparam>
		/// <returns></returns>
		public static IQueryHandler<TIn, TOut> Query<TIn, TOut>()
			where TIn : IQuery<TOut>
		{
			return new NullQueryHandler<TIn, TOut>();
		}

		/// <summary>
		///     Returns dummy command handler.
		/// </summary>
		/// <typeparam name="TIn">The type of command to be handled.</typeparam>
		/// <typeparam name="TOut">The type of result from the handler.</typeparam>
		/// <returns></returns>
		public static ICommandHandler<TIn, TOut> Command<TIn, TOut>()
			where TIn : ICommand<TOut>
		{
			return new NullCommandHandler<TIn, TOut>();
		}

		/// <summary>
		///     Returns dummy notification handler.
		/// </summary>
		/// <typeparam name="TIn">The type of notification to be handled.</typeparam>
		/// <returns></returns>
		public static INotificationHandler<TIn> Notification<TIn>()
			where TIn : INotification
		{
			return new NullNotificationHandler<TIn>();
		}
	}
}