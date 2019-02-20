using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.CQRS.Handlers
{
	public interface IRequestHandler<in TIn, TOut>
		where TIn : IRequest<TOut>
	{
		/// <summary>
		///     Handles request asynchronously and returns result.
		/// </summary>
		/// <param name="request">Request to be handled.</param>
		/// <param name="cancellationToken">The token that used for propagation notification that task should be canceled.</param>
		/// <returns></returns>
		Task<TOut> HandleAsync(TIn request, CancellationToken cancellationToken);
	}
}