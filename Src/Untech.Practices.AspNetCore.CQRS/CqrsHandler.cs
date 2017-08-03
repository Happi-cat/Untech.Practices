using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Untech.Practices.AspNetCore.CQRS.Mappers;
using Untech.Practices.CQRS;
using Untech.Practices.CQRS.Dispatching;

namespace Untech.Practices.AspNetCore.CQRS
{
	public abstract class CqrsHandler<TIn, TOut> : IRouter
	{
		private readonly IRequestMapper<TIn> _requestBinder;
		private readonly IResponseMapper<TOut> _responseBinder;

		protected CqrsHandler(IRequestMapper<TIn> requestBinder, IResponseMapper<TOut> responseBinder)
		{
			_requestBinder = requestBinder;
			_responseBinder = responseBinder;
		}

		protected async Task Handle(HttpContext context)
		{
			var dispatcher = context.RequestServices.GetRequiredService<IDispatcher>();
			var request = _requestBinder.Map(context.Request);

			var response = await HandleCore(dispatcher, request, context.RequestAborted);

			_responseBinder.Map(response, context.Response);
		}

		protected abstract Task<TOut> HandleCore(IDispatcher dispatcher, TIn data, CancellationToken cancellationToken);

		public Task RouteAsync(RouteContext context)
		{
			context.Handler = Handle;
			return Task.CompletedTask;
		}

		public VirtualPathData GetVirtualPath(VirtualPathContext context)
		{
			return null;
		}
	}
}
