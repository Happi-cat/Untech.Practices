using System.Threading;
using System.Threading.Tasks;

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
		/// <inheritdoc />
		public TOut Handle(TIn query) => default(TOut);

		/// <inheritdoc />
		public Task<TOut> HandleAsync(TIn query, CancellationToken cancellationToken) => Task.FromResult(default(TOut));
	}
}