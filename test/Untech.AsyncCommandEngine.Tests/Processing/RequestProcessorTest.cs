using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Untech.AsyncCommandEngine.Metadata;
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
			var processor = new RequestProcessor(new []{ new FakeMiddleware() });
			var context = new Context(new FakeRequest(), NullRequestMetadata.Instance);

			await processor.InvokeAsync(context);
		}

		private class FakeMiddleware : IRequestProcessorMiddleware
		{
			public Task InvokeAsync(Context context, RequestProcessorCallback next)
			{
				return Task.FromResult(0);
			}
		}

		private class FakeRequest : Request
		{
			public override string Identifier { get; }
			public override string Name { get; }
			public override DateTimeOffset Created { get; }
			public override IDictionary<string, string> Attributes { get; }
			public override object GetBody(Type requestType)
			{
				throw new NotImplementedException();
			}

			public override Stream GetRawBody()
			{
				throw new NotImplementedException();
			}
		}
	}
}