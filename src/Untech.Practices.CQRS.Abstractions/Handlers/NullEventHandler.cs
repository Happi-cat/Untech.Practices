using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.CQRS.Handlers
{
	/// <summary>
	///     Represents dummy notification handler.
	/// </summary>
	/// <typeparam name="TIn">The type of notification to be handled.</typeparam>
	public sealed class NullEventHandler<TIn> :
		IEventHandler<TIn>
		where TIn : IEvent
	{
		public static readonly NullEventHandler<TIn> Instance = new NullEventHandler<TIn>();

		private NullEventHandler()
		{

		}

		/// <inheritdoc />
		public Task PublishAsync(TIn notification, CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}