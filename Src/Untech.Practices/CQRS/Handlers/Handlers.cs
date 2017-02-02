using System;
using Untech.Practices.CQRS.Requests;

namespace Untech.Practices.CQRS.Handlers
{
	public static class Handlers
	{
		/// <summary>
		/// Returns dummy query handler.
		/// </summary>
		/// <typeparam name="TIn">The type of query to be handled.</typeparam>
		/// <typeparam name="TOut">The type of result from the handler.</typeparam>
		/// <returns></returns>
		public static IQueryHandler<TIn, TOut> Query<TIn, TOut>()
			   where TIn : IQuery<TOut>
		{
			return new StubQueryHandler<TIn, TOut>();
		}

		/// <summary>
		/// Returns query handler from <see cref="Func{T, TResult}"/>
		/// </summary>
		/// <typeparam name="TIn">The type of query to be handled.</typeparam>
		/// <typeparam name="TOut">The type of result from the handler.</typeparam>
		/// <param name="func"></param>
		/// <returns></returns>
		public static IQueryHandler<TIn, TOut> Query<TIn, TOut>(Func<TIn, TOut> func) 
			where TIn : IQuery<TOut>
		{
			return new AdHocQueryHandler<TIn, TOut>(func);
		}

		/// <summary>
		/// Returns dummy command handler.
		/// </summary>
		/// <typeparam name="TIn">The type of command to be handled.</typeparam>
		/// <typeparam name="TOut">The type of result from the handler.</typeparam>
		/// <returns></returns>
		public static ICommandHandler<TIn, TOut> Command<TIn, TOut>()
			where TIn : ICommand<TOut>
		{
			return new StubCommandHandler<TIn, TOut>();
		}

		/// <summary>
		/// Returns command handler from <see cref="Func{T, TResult}"/>
		/// </summary>
		/// <typeparam name="TIn">The type of command to be handled.</typeparam>
		/// <typeparam name="TOut">The type of result from the handler.</typeparam>
		/// <param name="func"></param>
		/// <returns></returns>
		public static ICommandHandler<TIn, TOut> Command<TIn, TOut>(Func<TIn, TOut> func) 
			where TIn : ICommand<TOut>
		{
			return new AdHocCommandHandler<TIn, TOut>(func);
		}

		/// <summary>
		/// Returns dummy notification handler.
		/// </summary>
		/// <typeparam name="TIn">The type of notification to be handled.</typeparam>
		/// <returns></returns>
		public static INotificationHandler<TIn> Notification<TIn>()
			where TIn : INotification
		{
			return new StubNotificationHandler<TIn>();
		}

		/// <summary>
		/// Returns notification handler from <see cref="Action{T}"/>
		/// </summary>
		/// <typeparam name="TIn">The type of notification to be handled.</typeparam>
		/// <param name="func"></param>
		/// <returns></returns>
		public static INotificationHandler<TIn> Notification<TIn>(Action<TIn> func)
			where TIn : INotification
		{
			return new AdHocNotificationHandler<TIn>(func);
		}
	}
}