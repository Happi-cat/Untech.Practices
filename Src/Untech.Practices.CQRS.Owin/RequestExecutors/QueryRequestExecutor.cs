using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Owin.RequestMappers;

namespace Untech.Practices.CQRS.Owin.RequestExecutors
{
	internal class QueryRequestExecutor<TIn, TOut> : IRequestExecutor
		where TIn : IQuery<TOut>
	{
		public QueryRequestExecutor(IRequestMapper<TIn> mapper)
		{
			Mapper = mapper;
		}

		public IRequestMapper<TIn> Mapper { get; }
		public Type RequestType => typeof(TIn);
		public Type ResponseType => typeof(TOut);

		public Task Handle(IOwinContext context, IDispatcher dispatcher)
		{
			return dispatcher.FetchAsync(Mapper.Map(context.Request));
		}
	}
}