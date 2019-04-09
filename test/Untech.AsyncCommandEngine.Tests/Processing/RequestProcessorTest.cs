using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Untech.AsyncCommandEngine.Metadata;
using Untech.Practices.CQRS;
using Untech.Practices.CQRS.Dispatching;
using Xunit;

namespace Untech.AsyncCommandEngine.Processing
{
	public class RequestProcessorTest
	{
		[Fact]
		public async Task InvokeAsync_ThrowsNotSupported_WhenNextCalledFromFinalMiddleware()
		{
			var processor = new RequestProcessor(Enumerable.Empty<IRequestProcessorMiddleware>());
			var context = new Context(new FakeRequest(), NullRequestMetadata.Instance);

			await Assert.ThrowsAsync<NotSupportedException>(() => processor.InvokeAsync(context));
		}

		[Fact]
		public async Task InvokeAsync_Returns_WhenFinalMiddlewareEnds()
		{
			var processor = new RequestProcessor(new[]{ new FakeMiddleware() });
			var context = new Context(new FakeRequest(), NullRequestMetadata.Instance);

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
			return new EngineBuilder().BuildProcessor(new FakeCqrsStrategy());
		}

		private static Context BuildContextForCqrsMiddlewareTest(object command)
		{
			return new Context(new FakeRequest(command), NullRequestMetadata.Instance);
		}

		private class FakeMiddleware : IRequestProcessorMiddleware
		{
			public Task InvokeAsync(Context context, RequestProcessorCallback next)
			{
				return Task.CompletedTask;
			}
		}

		private class FakeCqrsStrategy : ICqrsStrategy, IDispatcher
		{
			private readonly IReadOnlyList<Type> _types = new List<Type>
			{
				typeof(FakeCommand), typeof(FakeCommandWithoutInterface), typeof(FakeCommandWithMultipleInterfaces)
			};

			public Type FindRequestType(string requestName)
			{
				return _types.FirstOrDefault(t => t.FullName == requestName);
			}

			public IDispatcher GetDispatcher(Context context)
			{
				return this;
			}

			public Task<TResult> FetchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken)
			{
				return Task.FromResult(default(TResult));
			}

			public Task PublishAsync(INotification notification, CancellationToken cancellationToken)
			{
				return Task.CompletedTask;
			}

			public Task<TResult> ProcessAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken)
			{
				return Task.FromResult(default(TResult));
			}
		}

		private class FakeRequest : Request
		{
			private readonly object _body;

			public FakeRequest() : this(new FakeCommand())
			{
			}

			public FakeRequest(object body)
			{
				Identifier = Guid.NewGuid().ToString();
				Name = body.GetType().FullName;
				Created = DateTimeOffset.Now;
				_body = body;
			}

			public override string Identifier { get; }
			public override string Name { get; }
			public override DateTimeOffset Created { get; }
			public override IDictionary<string, string> Attributes { get; }

			public override object GetBody(Type requestType)
			{
				return _body;
			}

			public override Stream GetRawBody()
			{
				throw new NotImplementedException();
			}
		}

		private class FakeCommand : ICommand
		{
		}

		private class FakeCommandWithoutInterface
		{
		}

		private class FakeCommandWithMultipleInterfaces : ICommand<int>, ICommand<string>
		{
		}
	}
}