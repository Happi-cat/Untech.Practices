using System.Threading.Tasks;
using Untech.AsyncCommandEngine.Metadata;
using Untech.AsyncCommandEngine.Processing;

namespace Untech.AsyncCommandEngine.Features.Debounce
{
	internal class DebounceMiddleware : IRequestProcessorMiddleware
	{
		private readonly ILastRunStore _lastRunStore;

		public DebounceMiddleware(ILastRunStore lastRunStore)
		{
			_lastRunStore = lastRunStore;
		}

		public async Task InvokeAsync(Context context, RequestProcessorCallback next)
		{
			var debounceAttribute = context.RequestMetadata.GetAttribute<DebounceAttribute>();

			if (debounceAttribute != null)
			{
				var lastRun = await _lastRunStore.GetLastRunAsync(context.Request, context.Aborted);
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