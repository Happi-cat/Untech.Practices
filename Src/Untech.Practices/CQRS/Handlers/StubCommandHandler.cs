using System.Threading.Tasks;
using Untech.Practices.CQRS.Requests;

namespace Untech.Practices.CQRS.Handlers
{
	/// <summary>
	/// Represents dummy command handler that returns default value.
	/// </summary>
	/// <typeparam name="TIn">Request type.</typeparam>
	/// <typeparam name="TOut">The type of result from the handler.</typeparam>
	public sealed class StubCommandHandler<TIn, TOut> :
		ICommandHandler<TIn, TOut>, ICommandAsyncHandler<TIn, TOut>
		where TIn : ICommand<TOut>
	{
		public TOut Handle(TIn command) => default(TOut);

		public Task<TOut> HandleAsync(TIn command) => Task.FromResult(default(TOut));
	}
}