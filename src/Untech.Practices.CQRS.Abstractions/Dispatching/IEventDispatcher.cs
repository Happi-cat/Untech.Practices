using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.CQRS.Dispatching
{
	/// <summary>
	///     Represents interface of a class that dispatches CQRS notifications to handlers.
	/// </summary>
	/// <remarks>
	///     <para>
	///         Supports <see cref="IEvent" /> CQRS request.
	///     </para>
	/// </remarks>
	public interface IEventDispatcher
	{
		/// <summary>
		///     Publishes asynchronously the incoming <paramref name="event" />.
		/// </summary>
		/// <param name="event">The command to be published.</param>
		/// <param name="cancellationToken">The token that used for propagation notification that task should be canceled.</param>
		Task PublishAsync(IEvent @event, CancellationToken cancellationToken);
	}
}