using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Untech.AsyncJob.Fakes;
using Untech.AsyncJob.Metadata.Annotations;
using Xunit;

namespace Untech.AsyncJob.Features.WatchDog
{
	public class WatchDogMiddlewareTest
	{
		[Fact]
		public async Task InvokeAsync_Completes_WhenNoWatchdog()
		{
			await InvokeAsync(TimeSpan.Zero, null, null);
		}

		[Fact]
		public async Task InvokeAsync_Completes_WhenCommandEndsBeforeDefaultWatchdog()
		{
			await InvokeAsync(TimeSpan.Zero, TimeSpan.FromSeconds(1), null);
		}

		[Fact]
		public async Task InvokeAsync_Completes_WhenCommandEndsBeforeRequestWatchdog()
		{
			await InvokeAsync(TimeSpan.Zero, null, TimeSpan.FromSeconds(1));
		}

		[Fact]
		public async Task InvokeAsync_Terminates_WhenCommandEndsAfterDefaultWatchdog()
		{
			await Assert.ThrowsAsync<TaskCanceledException>(() => InvokeAsync(
				TimeSpan.FromSeconds(2),
				TimeSpan.FromSeconds(1),
				null));
		}

		[Fact]
		public async Task InvokeAsync_Terminates_WhenCommandEndsAfterRequestWatchdog()
		{
			await Assert.ThrowsAsync<TaskCanceledException>(() => InvokeAsync(
				TimeSpan.FromSeconds(2),
				null,
				TimeSpan.FromSeconds(1)));
		}

		private static async Task InvokeAsync(TimeSpan commandDuration, TimeSpan? defaultTimeout, TimeSpan? requestTimeout)
		{
			var middleware = new WatchDogMiddleware(new WatchDogOptions
			{
				DefaultTimeout = defaultTimeout
			}, NullLoggerFactory.Instance);
			var meta = requestTimeout != null
				? new[] { new WatchDogTimeoutAttribute(requestTimeout.Value.Seconds) }
				: null;

			await middleware.InvokeAsync(new Context(new FakeRequest(meta: meta)), async ctx =>
			{
				await Task.Delay(commandDuration, ctx.Aborted);
			});
		}
	}
}
