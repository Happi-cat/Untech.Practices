using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.CQRS.Handlers
{
	/// <summary>
	///     Represents dummy command handler that returns default value.
	/// </summary>
	/// <typeparam name="TIn">Request type.</typeparam>
	/// <typeparam name="TOut">The type of result from the handler.</typeparam>
	public sealed class NullCommandHandler<TIn, TOut> :
		ICommandHandler<TIn, TOut>, ICommandAsyncHandler<TIn, TOut>
		where TIn : ICommand<TOut>
	{
		/// <inheritdoc />
		public Task<TOut> HandleAsync(TIn command, CancellationToken cancellationToken)
		{
			return Task.FromResult(default(TOut));
		}

		/// <inheritdoc />
		public TOut Handle(TIn request)
		{
			return default;
		}
	}
}