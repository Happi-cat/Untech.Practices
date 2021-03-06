﻿namespace Untech.Practices.CQRS.Handlers
{
	/// <summary>
	///     Defines an asynchronous handler for a query.
	/// </summary>
	/// <typeparam name="TIn">The type of query to be handled.</typeparam>
	/// <typeparam name="TOut">The type of result from the handler.</typeparam>
	public interface IQueryHandler<TIn, TOut> : IRequestHandler<TIn, TOut>
		where TIn : IQuery<TOut>
	{
	}
}