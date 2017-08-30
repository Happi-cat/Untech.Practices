using System.Threading;
using System.Threading.Tasks;
using Untech.Practices.AspNetCore.CQRS.Mappers;
using Untech.Practices.CQRS;
using Untech.Practices.CQRS.Dispatching;

namespace Untech.Practices.AspNetCore.CQRS
{
	public class NotificationAsync<TIn> : CqrsHandler<TIn, Nothing>
		where TIn : INotification
	{
		public NotificationAsync(IRequestMapper<TIn> requestBinder, IResponseMapper<Nothing> responseBinder) : base(requestBinder, responseBinder)
		{
		}

		protected override async Task<Nothing> HandleCore(IDispatcher dispatcher, TIn data, CancellationToken cancellationToken)
		{
			await dispatcher.PublishAsync(data, cancellationToken);

			return Nothing.AtAll;
		}
	}
}
