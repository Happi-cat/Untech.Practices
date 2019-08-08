using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Untech.Practices.CQRS.Handlers;

namespace Untech.Practices.CQRS.Dispatching.Processors
{
	internal class EventProcessor<TIn> : IProcessor
		where TIn : IEvent
	{
		private readonly TypeResolver _resolver;

		public EventProcessor(TypeResolver resolver)
		{
			_resolver = resolver;
		}

		public Task InvokeAsync(object args, CancellationToken cancellationToken)
		{
			TIn input = (TIn)args;

			IEnumerable<Task> asyncHandlers = _resolver
				.ResolveMany<IEventHandler<TIn>>()
				.Select(RunAsync);

			return Task.WhenAll(asyncHandlers);

			Task RunAsync(IEventHandler<TIn> handler)
			{
				return handler.PublishAsync(input, cancellationToken);
			}
		}
	}
}