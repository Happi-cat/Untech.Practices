using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Untech.AsyncJob.Builder;
using Untech.AsyncJob.Fakes;
using Untech.AsyncJob.Features.CQRS;
using Untech.AsyncJob.Formatting;
using Untech.AsyncJob.Formatting.Json;
using Untech.Practices.CQRS;
using Untech.Practices.CQRS.Dispatching;
using Xunit;

namespace Untech.AsyncJob.Processing
{
	public class RequestProcessorTest
	{
		[Fact]
		public async Task InvokeAsync_ThrowsInvalidOperation_WhenNextCalledFromFinalMiddleware()
		{
			var processor = new RequestProcessor(Enumerable.Empty<IRequestProcessorMiddleware>());
			var context = new Context(new FakeRequest());

			await Assert.ThrowsAsync<InvalidOperationException>(() => processor.InvokeAsync(context));
		}

		[Fact]
		public async Task InvokeAsync_Returns_WhenFinalMiddlewareEnds()
		{
			var processor = new RequestProcessor(new[] { NullRequestProcessorMiddleware.Instance });
			var context = new Context(new FakeRequest());

			await processor.InvokeAsync(context);
		}

		[Fact]
		public async Task CqrsInvokeAsync_Returns_WhenCommandFound()
		{
			var processor = BuildProcessorForCqrsMiddlewareTest();
			var context = BuildContextForCqrsMiddlewareTest(new FakeCommand());

			await processor.InvokeAsync(context);
		}

		[Fact]
		public async Task CqrsInvokeAsync_ThrowsInvalidOperation_WhenCommandNotFound()
		{
			var processor = BuildProcessorForCqrsMiddlewareTest();
			var context = BuildContextForCqrsMiddlewareTest(new object());

			await Assert.ThrowsAsync<InvalidOperationException>(() => processor.InvokeAsync(context));
		}

		[Fact]
		public async Task CqrsInvokeAsync_ThrowsInvalidOperation_WhenCommandHasNoProperInterfaces()
		{
			var processor = BuildProcessorForCqrsMiddlewareTest();
			var context = BuildContextForCqrsMiddlewareTest(new FakeCommandWithoutInterface());

			await Assert.ThrowsAsync<InvalidOperationException>(() => processor.InvokeAsync(context));
		}

		[Fact]
		public async Task CqrsInvokeAsync_ThrowsInvalidOperation_WhenCommandHasMultipleInterfaces()
		{
			var processor = BuildProcessorForCqrsMiddlewareTest();
			var context = BuildContextForCqrsMiddlewareTest(new FakeCommandWithMultipleInterfaces());

			await Assert.ThrowsAsync<InvalidOperationException>(() => processor.InvokeAsync(context));
		}

		private static IRequestProcessor BuildProcessorForCqrsMiddlewareTest()
		{
			var types = new[]
			{
				typeof(FakeCommand), typeof(FakeCommandWithoutInterface), typeof(FakeCommandWithMultipleInterfaces)
			};

			return new EngineBuilder()
				.Finally(new CqrsStrategy(new RequestTypeFinder(types))
				{
					Dispatcher = new FakeDispatcher(),
					Formatter = JsonRequestContentFormatter.Default
				})
				.BuildProcessor();
		}

		private static Context BuildContextForCqrsMiddlewareTest(object command)
		{
			return new Context(new FakeRequest(body: command));
		}

		private class FakeDispatcher : IDispatcher
		{
			public Task<TResult> FetchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken)
			{
				return Task.FromResult(default(TResult));
			}

			public Task PublishAsync(IEvent @event, CancellationToken cancellationToken)
			{
				return Task.CompletedTask;
			}

			public Task<TResult> ProcessAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken)
			{
				return Task.FromResult(default(TResult));
			}
		}

		public class FakeCommand : ICommand
		{
		}

		public class FakeCommandWithoutInterface
		{
		}

		public class FakeCommandWithMultipleInterfaces : ICommand<int>, ICommand<string>
		{
		}
	}
}
