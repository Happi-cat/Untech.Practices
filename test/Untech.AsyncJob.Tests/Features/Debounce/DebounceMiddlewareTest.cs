using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Untech.AsyncJob.Fakes;
using Untech.AsyncJob.Metadata.Annotations;
using Xunit;

namespace Untech.AsyncJob.Features.Debounce
{
	public class DebounceMiddlewareTest
	{
		[Fact]
		public async Task InvokeAsync_NonDebounced_WhenFirstRunAndDebounceEnabled()
		{
			var created = DateTimeOffset.Now;
			var invoked = await InvokeAsync(created, DateTimeOffset.MinValue, true);

			Assert.True(invoked);
		}

		[Fact]
		public async Task InvokeAsync_NonDebounced_WhenCreatedAfterLastRunAndDebounceEnabled()
		{
			var created = DateTimeOffset.Now;
			var invoked = await InvokeAsync(created, created.AddMinutes(-1), true);

			Assert.True(invoked);
		}

		[Fact]
		public async Task InvokeAsync_NonDebounced_WhenCreatedBeforeLastRunAndDebounceDisabled()
		{
			var created = DateTimeOffset.Now;
			var invoked = await InvokeAsync(created, created.AddMinutes(1), false);

			Assert.True(invoked);
		}

		[Fact]
		public async Task InvokeAsync_Debounced_WhenCreatedBeforeLastRunAndDebounceEnabled()
		{
			var created = DateTimeOffset.Now;
			var invoked = await InvokeAsync(created, created.AddMinutes(1), true);

			Assert.False(invoked);
		}

		private static async Task<bool> InvokeAsync(DateTimeOffset created, DateTimeOffset lastRun, bool debounce)
		{
			var lastRunStore = new FakeLastRunStore(lastRun);
			var middleware = new DebounceMiddleware(lastRunStore, NullLoggerFactory.Instance);
			var meta = debounce ? new[] { new DebounceAttribute() } : null;
			var request = new FakeRequest(created: created, meta: meta);

			bool executed = false;

			await middleware.InvokeAsync(new Context(request), ctx =>
			{
				executed = true;
				return Task.CompletedTask;
			});

			return executed;
		}

		private class FakeLastRunStore : ILastRunStore
		{
			private DateTimeOffset _lastRun;

			public FakeLastRunStore(DateTimeOffset lastRun)
			{
				_lastRun = lastRun;
			}

			public Task<DateTimeOffset> GetLastRunAsync(Request request, CancellationToken cancellationToken)
			{
				return Task.FromResult(_lastRun);
			}

			public Task SetLastRunAsync(Request request)
			{
				_lastRun = DateTimeOffset.Now;
				return Task.CompletedTask;
			}
		}
	}
}
