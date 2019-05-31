using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.CQRS.Handlers
{
	/// <summary>
	///     Represents dummy query handler that returns default value.
	/// </summary>
	/// <typeparam name="TIn">Request type.</typeparam>
	/// <typeparam name="TOut">The type of result from the handler.</typeparam>
	public sealed class NullQueryHandler<TIn, TOut> :
		IQueryHandler<TIn, TOut>
		where TIn : IQuery<TOut>
	{
		public static readonly NullQueryHandler<TIn, TOut> Instance = new NullQueryHandler<TIn, TOut>();

		private NullQueryHandler()
		{

		}

		/// <inheritdoc />
		public Task<TOut> HandleAsync(TIn query, CancellationToken cancellationToken)
		{
			return Task.FromResult(default(TOut));
		}
	}
}