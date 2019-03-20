using System.Threading.Tasks;
using Untech.AsyncCommmandEngine.Abstractions;

namespace AsyncCommandEngine.Examples.Features.Debounce
{
	internal class DebounceMiddleware : IAceProcessorMiddleware
	{
		private readonly ILastRunStore _lastRunStore;

		public DebounceMiddleware(ILastRunStore lastRunStore)
		{
			_lastRunStore = lastRunStore;
		}

		public async Task Execute(AceContext context, AceRequestProcessorDelegate next)
		{
			var debounceAttribute = context.Request.Metadata.GetAttribute<DebounceAttribute>();

			if (debounceAttribute != null)
			{
				var lastRun = await _lastRunStore.GetLastRunAsync(context.Request, context.RequestAborted);
				if (lastRun != null && context.Request.Created < lastRun)
					return;

				try
				{
					await next(context);
				}
				finally
				{
					await _lastRunStore.SetLastRunAsync(context.Request);
				}
			}
			else
			{
				await next(context);
			}
		}
	}
}