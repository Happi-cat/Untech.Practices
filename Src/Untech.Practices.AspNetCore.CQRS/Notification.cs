using System.Threading;
using System.Threading.Tasks;
using Untech.Practices.AspNetCore.CQRS.Mappers;
using Untech.Practices.CQRS;

namespace Untech.Practices.AspNetCore.CQRS
{
	public static class Notification
	{
		public static Notification<TIn> Use<TIn>(IRequestMapper<TIn> requestBinder, IResponseMapper<Unit> responseBinder)
			where TIn : INotification
		{
			return new Notification<TIn>(requestBinder, responseBinder);
		}

		public static NotificationAsync<TIn> UseAsync<TIn>(IRequestMapper<TIn> requestBinder, IResponseMapper<Unit> responseBinder)
			where TIn : INotification
		{
			return new NotificationAsync<TIn>(requestBinder, responseBinder);
		}
	}

	public class Notification<TIn> : CqrsHandler<TIn, Unit>
		where TIn : INotification
	{
		public Notification(IRequestMapper<TIn> requestBinder, IResponseMapper<Unit> responseBinder) : base(requestBinder, responseBinder)
		{
		}

		protected override Task<Unit> HandleCore(IDispatcher dispatcher, TIn data, CancellationToken cancellationToken)
		{
			dispatcher.Publish(data);
			return Task.FromResult(Unit.Value);
		}
	}
}
