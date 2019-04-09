using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Untech.AsyncCommandEngine.Processing;
using Untech.Practices.CQRS;
using Untech.Practices.CQRS.Dispatching;
using Xunit;

namespace Untech.AsyncCommandEngine
{
	public class OrchestratorTest
	{
		[Fact]
		public async Task DoAsync_ConsumesRequests_WhenRequestsAvailable()
		{
			var transport = new FakeTransport();
			var orcherstator = new EngineBuilder().UseTransport(transport).BuildOrchestrator(new FakeCqrs(), new OrchestratorOptions
			{
				Warps = 5,
				RequestsPerWarp = 5,
				RunRequestsInWarpAllAtOnce = true,
				SlidingStep = TimeSpan.FromMilliseconds(10)
			});

			await orcherstator.StartAsync();
			await transport.Complete();
			await orcherstator.StopAsync(TimeSpan.Zero);
		}

		private class FakeTransport : ITransport
		{
			private readonly int _total;
			private readonly ConcurrentBag<Request> _inbound;
			private readonly ConcurrentBag<Request> _outbound;
			private readonly TaskCompletionSource<int> _completionSource;

			public FakeTransport()
			{
				_total = 100;
				_inbound = new ConcurrentBag<Request>(Enumerable.Repeat(new FakeRequest(), _total));
				_outbound = new ConcurrentBag<Request>();
				_completionSource = new TaskCompletionSource<int>();
			}

			public Task<ReadOnlyCollection<Request>> GetRequestsAsync(int count)
			{
				var requests = new List<Request>();

				while (count-- > 0 && _inbound.TryTake(out var request)) requests.Add(request);

				return Task.FromResult(requests.AsReadOnly());
			}

			public Task CompleteRequestAsync(Request request)
			{
				_outbound.Add(request);
				if (_outbound.Count == _total) _completionSource.SetResult(_outbound.Count);
				return Task.CompletedTask;
			}

			public Task FailRequestAsync(Request request, Exception exception)
			{
				return CompleteRequestAsync(request);
			}

			public Task Complete()
			{
				return _completionSource.Task;
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
				return new FakeCommand();
			}

			public override Stream GetRawBody()
			{
				throw new NotImplementedException();
			}
		}

		private class FakeCqrs : ICqrsStrategy, IDispatcher
		{
			public Type FindRequestType(string requestName)
			{
				return typeof(FakeCommand);
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

		private class FakeCommand : ICommand
		{

		}
	}
}