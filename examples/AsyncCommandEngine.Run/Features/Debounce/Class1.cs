using System;
using System.Threading;
using System.Threading.Tasks;
using AsyncCommandEngine.Run;
using Untech.AsyncCommmandEngine.Abstractions;

namespace AsyncCommandEngine.Examples.Features.Debounce
{
	public static class AceBuilderExtensions
	{
		public static AceBuilder UseDebounce(this AceBuilder builder, ILastRunStore lastRunStore)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (lastRunStore == null) throw new ArgumentNullException(nameof(lastRunStore));

			return builder.Use(() => new DebounceMiddleware(lastRunStore));
		}
	}

	[AttributeUsage(AttributeTargets.Class)]
	public class DebounceAttribute : Attribute
	{

	}

	public interface ILastRunStore
	{
		Task<DateTimeOffset?> GetLastRunAsync(AceRequest request, CancellationToken cancellationToken);
		Task SetLastRunAsync(AceRequest request);
	}

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