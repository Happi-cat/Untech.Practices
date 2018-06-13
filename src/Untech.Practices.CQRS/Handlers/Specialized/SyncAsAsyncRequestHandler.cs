using System;
using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.CQRS.Handlers.Specialized
{
	public class SyncAsAsyncRequestHandler<TIn, TOut> : IRequestAsyncHandler<TIn, TOut>
		where TIn : IRequest<TOut>
	{
		private readonly IRequestHandler<TIn, TOut> _handler;

		public SyncAsAsyncRequestHandler(IRequestHandler<TIn, TOut> handler)
		{
			_handler = handler ?? throw new ArgumentNullException(nameof(handler));
		}

		public Task<TOut> HandleAsync(TIn request, CancellationToken cancellationToken)
		{
			return Task.FromResult(_handler.Handle(request));
		}
	}
}