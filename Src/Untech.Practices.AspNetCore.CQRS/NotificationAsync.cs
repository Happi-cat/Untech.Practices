using System.Threading;
using System.Threading.Tasks;
using Untech.Practices.AspNetCore.CQRS.Mappers;
using Untech.Practices.CQRS;
using Untech.Practices.CQRS.Dispatching;

namespace Untech.Practices.AspNetCore.CQRS
{
	public class NotificationAsync<TIn> : CqrsHandler<TIn, Unit>
		where TIn : INotification
	{
		public NotificationAsync(IRequestMapper<TIn> requestBinder, IResponseMapper<Unit> responseBinder) : base(requestBinder, responseBinder)
		{
		}

		protected override async Task<Unit> HandleCore(IDispatcher dispatcher, TIn data, CancellationToken cancellationToken)
		{
			await dispatcher.PublishAsync(data, cancellationToken);

			return Unit.Value;
		}
	}
}
