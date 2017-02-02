using System.Threading.Tasks;
using Untech.Practices.CQRS.Requests;

namespace Untech.Practices.CQRS.Handlers
{
	/// <summary>
	/// Represents dummy query handler that returns default value.
	/// </summary>
	/// <typeparam name="TIn">Request type.</typeparam>
	/// <typeparam name="TOut">The type of result from the handler.</typeparam>
	public sealed class StubQueryHandler<TIn, TOut> :
		IQueryHandler<TIn, TOut>, IQueryAsyncHandler<TIn, TOut> 
		where TIn : IQuery<TOut>
	{
		public TOut Fetch(TIn command) => default(TOut);

		public Task<TOut> FetchAsync(TIn command) => Task.FromResult(default(TOut));
	}
}