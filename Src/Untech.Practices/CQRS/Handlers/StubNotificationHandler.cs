using System.Threading.Tasks;
using Untech.Practices.CQRS.Requests;

namespace Untech.Practices.CQRS.Handlers
{
	/// <summary>
	/// Represents dummy notification handler.
	/// </summary>
	/// <typeparam name="TIn">The type of notification to be handled.</typeparam>
	public sealed class StubNotificationHandler<TIn> :
		INotificationHandler<TIn>, INotificationAsyncHandler<TIn> 
		where TIn : INotification
	{
		public void Publish(TIn command) { }

		public Task PublishAsync(TIn command) => Task.CompletedTask;
	}
}