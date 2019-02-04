namespace Untech.Practices.CQRS.Handlers
{
	/// <summary>
	///     Defines a handler for a query.
	/// </summary>
	/// <typeparam name="TIn">The type of query to be handled.</typeparam>
	/// <typeparam name="TOut">The type of result from the handler.</typeparam>
	public interface IQueryHandler<in TIn, out TOut> : IRequestHandler<TIn, TOut>
		where TIn : IQuery<TOut>
	{
	}
}