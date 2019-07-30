using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Untech.AsyncJob.Fakes;
using Xunit;

namespace Untech.AsyncJob.Features.Retrying
{
	public class RetryingMiddlewareTest
	{
		[Fact]
		public async Task InvokeAsync_NoRetry_WhenRetryCountIsZero()
		{
			var policy = new RetryPolicy(0, new List<Type>());

			var invoked = await InvokeAsync(policy);

			Assert.Equal(1, invoked);
		}

		[Fact]
		public async Task InvokeAsync_RetryOne_WhenRetryCountIsOneAndExceptionInList()
		{
			var policy = new RetryPolicy(new List<Type>
			{
				typeof(TimeoutException)
			});

			var invoked = await InvokeAsync(policy);

			Assert.Equal(2, invoked);
		}

		[Fact]
		public async Task InvokeAsync_RetryTwice_WhenRetryCountIsTwoAndExceptionInList()
		{
			var policy = new RetryPolicy(2, new List<Type>
			{
				typeof(TimeoutException)
			});

			var invoked = await InvokeAsync(policy);

			Assert.Equal(3, invoked);
		}

		[Fact]
		public async Task InvokeAsync_NoRetry_WhenExceptionNotInList()
		{
			var policy = new RetryPolicy(new List<Type>
			{
				typeof(KeyNotFoundException)
			});

			var invoked = await InvokeAsync(policy);

			Assert.Equal(1, invoked);
		}


		private static async Task<int> InvokeAsync(IRetryPolicy retryPolicy)
		{
			var middleware = new RetryingMiddleware(retryPolicy, NullLoggerFactory.Instance);

			int invoked = 0;

			try
			{
				await middleware.InvokeAsync(new Context(new FakeRequest()), ctx =>
				{
					invoked++;
					throw new TimeoutException();
				});
			}
			catch (TimeoutException) { }

			return invoked;
		}
	}
}
