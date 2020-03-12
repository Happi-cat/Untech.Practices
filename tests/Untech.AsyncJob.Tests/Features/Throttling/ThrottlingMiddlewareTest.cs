using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Untech.AsyncJob.Fakes;
using Untech.AsyncJob.Processing;
using Xunit;

namespace Untech.AsyncJob.Features.Throttling
{
	public class ThrottleMiddlewareTest
	{
		[Fact]
		public async Task InvokeAsync_RunOnlyOne_WhenRunAtOnceIsOne()
		{
			await InvokeAsync(new ThrottleOptions { RunAtOnce = 1 });
		}

		[Fact]
		public async Task InvokeAsync_Throws_WhenRunAtOnceIsTwoButOneIsRequired()
		{
			await Assert.ThrowsAsync<SynchronizationLockException>(() => InvokeAsync(new ThrottleOptions { RunAtOnce = 2 }));
		}

		[Fact]
		public async Task InvokeAsync_RunOnlyOne_WhenRunAtOnceForGroupIsOne()
		{
			await InvokeAsync(new ThrottleOptions { DefaultRunAtOnceInGroup = 1 });
		}

		[Fact]
		public async Task InvokeAsync_Throws_WhenRunAtOnceForGroupIsTwoButOneIsRequired()
		{
			await Assert.ThrowsAsync<SynchronizationLockException>(() => InvokeAsync(new ThrottleOptions { DefaultRunAtOnceInGroup = 2 }));
		}

		[Fact]
		public async Task InvokeAsync_RunOnlyOne_WhenRunAtOnceForGroupIsOneAndRunAtOnceIsTwo()
		{
			await InvokeAsync(new ThrottleOptions { RunAtOnce = 2, DefaultRunAtOnceInGroup = 1 });
		}

		private static async Task InvokeAsync(ThrottleOptions options)
		{
			var middleware = new ThrottleMiddleware(options);

			RequestProcessorCallback next = async context =>
			{
				SimpleLock.Take();

				await Task.Delay(100);

				SimpleLock.Release();
			};

			var requests = Enumerable
				.Repeat(new Context(new FakeRequest()), 10)
				.Select(ctx => Task.Run(() => middleware.InvokeAsync(ctx, next)));

			await Task.WhenAll(requests);
		}

		private static class SimpleLock
		{
			private static volatile int s_sync;

			public static void Take()
			{
				if (TryTake())
					return;

				throw new SynchronizationLockException("Lock was already acquired");
			}

			public static void Release()
			{
				if (TryRelease())
					return;

				throw new SynchronizationLockException("Lock was already released");
			}

			// false when was taken by someone else
			private static bool TryTake()
			{
				return Interlocked.Exchange(ref s_sync, 1) == 0;
			}

			// false when was released by someone else
			private static bool TryRelease()
			{
				return Interlocked.Exchange(ref s_sync, 0) == 1;
			}
		}
	}
}
