using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Owin.RequestMappers;

namespace Untech.Practices.CQRS.Owin.RequestExecutors
{
	internal class NotificationRequestExecutor<TIn> : IRequestExecutor
		where TIn : INotification
	{
		public NotificationRequestExecutor(IRequestMapper<TIn> mapper)
		{
			Mapper = mapper;
		}

		public IRequestMapper<TIn> Mapper { get; }
		public Type RequestType => typeof(TIn);
		public Type ResponseType => typeof(Unit);

		public Task Handle(IOwinContext context, IDispatcher dispatcher)
		{
			return dispatcher.PublishAsync(Mapper.Map(context.Request));
		}
	}
}