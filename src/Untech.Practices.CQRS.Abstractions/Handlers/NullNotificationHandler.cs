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
		public static readonly NullNotificationHandler<TIn> Instance = new NullNotificationHandler<TIn>();

		private NullNotificationHandler()
		{

		}

		/// <inheritdoc />
		public Task PublishAsync(TIn notification, CancellationToken cancellationToken)
		{
			return Task.FromResult(0);
		}
	}
}