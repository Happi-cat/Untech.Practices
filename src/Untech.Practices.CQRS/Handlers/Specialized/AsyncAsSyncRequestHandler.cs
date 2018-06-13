using System;
using System.Threading;

namespace Untech.Practices.CQRS.Handlers.Specialized
{
	public class AsyncAsSyncRequestHandler<TIn, TOut> : IRequestHandler<TIn, TOut>
		where TIn : IRequest<TOut>
	{
		private readonly IRequestAsyncHandler<TIn, TOut> _handler;

		public AsyncAsSyncRequestHandler(IRequestAsyncHandler<TIn, TOut> handler)
		{
			_handler = handler ?? throw new ArgumentNullException(nameof(handler));
		}

		public TOut Handle(TIn request)
		{
			return _handler.HandleAsync(request, CancellationToken.None)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}
	}
}