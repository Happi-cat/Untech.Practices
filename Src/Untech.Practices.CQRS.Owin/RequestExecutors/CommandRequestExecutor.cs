using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Owin.RequestMappers;
using Untech.Practices.CQRS.Requests;

namespace Untech.Practices.CQRS.Owin.RequestExecutors
{
	internal class CommandRequestExecutor<TIn, TOut> : IRequestExecutor
		where TIn : ICommand<TOut>
	{
		public CommandRequestExecutor(IRequestMapper<TIn> mapper)
		{
			Mapper = mapper;
		}

		public IRequestMapper<TIn> Mapper { get; }
		public Type RequestType => typeof(TIn);
		public Type ResponseType => typeof(TOut);

		public Task Handle(IOwinContext context, IDispatcher dispatcher)
		{
			return dispatcher.ProcessAsync(Mapper.Map(context.Request));
		}
	}
}