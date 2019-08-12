using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Untech.Practices.CQRS.Handlers;

namespace Untech.Practices.CQRS.Dispatching.Processors
{
	internal static class EventProcessor<TIn> where TIn : IEvent
	{
		public static Task InvokeAsync(TypeResolver resolver, object args, CancellationToken cancellationToken)
		{
			TIn input = (TIn)args;

			IEnumerable<Task> asyncHandlers = resolver
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