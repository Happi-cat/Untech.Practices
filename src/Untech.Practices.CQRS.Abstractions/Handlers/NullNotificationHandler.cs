using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.CQRS.Handlers
{
	/// <summary>
	///     Represents dummy notification handler.
	/// </summary>
	/// <typeparam name="TIn">The type of notification to be handled.</typeparam>
	public sealed class NullNotificationHandler<TIn> :
		INotificationHandler<TIn>
		where TIn : INotification
	{
		/// <inheritdoc />
		public Task PublishAsync(TIn notification, CancellationToken cancellationToken)
		{
			return Task.FromResult(0);
		}
	}
}