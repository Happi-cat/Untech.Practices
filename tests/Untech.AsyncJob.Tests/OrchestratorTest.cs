﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Untech.AsyncJob.Builder;
using Untech.AsyncJob.Fakes;
using Untech.AsyncJob.Features.CQRS;
using Untech.AsyncJob.Formatting;
using Untech.AsyncJob.Formatting.Json;
using Untech.AsyncJob.Processing;
using Untech.AsyncJob.Transports;
using Untech.Practices.CQRS;
using Untech.Practices.CQRS.Dispatching;
using Xunit;

namespace Untech.AsyncJob
{
	public class OrchestratorTest
	{
		[Fact]
		public async Task DoAsync_ConsumesRequests_WhenRequestsAvailable()
		{
			var transport = new FakeTransport(100);
			var dispatcher = new FakeDispatcher();
			var cqrs = new CqrsStrategy(new RequestTypeFinder(new[] { typeof(FakeCommand) }))
			{
				Dispatcher = dispatcher, Formatter = new JsonRequestContentFormatter()
			};
			var orchestrator = new EngineBuilder(options =>
				{
					options.Warps = 5;
					options.RequestsPerWarp = 5;
					options.RunRequestsInWarpAllAtOnce = true;
					options.SlidingStep = TimeSpan.FromMilliseconds(10);
				})
				.ReceiveRequestsFrom(r => r.Add(transport))
				.Finally(cqrs)
				.BuildOrchestrator();

			await orchestrator.StartAsync();
			var completed = await transport.Complete();
			await orchestrator.StopAsync(TimeSpan.Zero);

			Assert.Equal(100, dispatcher.CallCounter);
			Assert.Equal(100, completed);
		}

		private class FakeTransport : ITransport
		{
			private readonly int _total;
			private readonly ConcurrentBag<Request> _inbound;
			private readonly ConcurrentBag<Request> _outbound;
			private readonly TaskCompletionSource<int> _completionSource;

			public FakeTransport(int total)
			{
				_total = total;
				_inbound = new ConcurrentBag<Request>(Enumerable.Repeat(new FakeRequest(body: new FakeCommand()), _total));
				_outbound = new ConcurrentBag<Request>();
				_completionSource = new TaskCompletionSource<int>();
			}

			public Task<Request[]> GetRequestsAsync(int count)
			{
				var requests = new List<Request>();

				while (count-- > 0 && _inbound.TryTake(out var request))
					requests.Add(request);

				return Task.FromResult(requests.ToArray());
			}

			public Task CompleteRequestAsync(Request request)
			{
				_outbound.Add(request);
				if (_outbound.Count == _total)
					_completionSource.TrySetResult(_outbound.Count);
				return Task.CompletedTask;
			}

			public Task FailRequestAsync(Request request, Exception exception)
			{
				return CompleteRequestAsync(request);
			}

			public Task Flush()
			{
				return Task.CompletedTask;
			}

			public Task<int> Complete()
			{
				return _completionSource.Task;
			}
		}

		private class FakeDispatcher : IDispatcher
		{
			private int _callCounter = 0;

			public int CallCounter => _callCounter;

			public Task<TResult> FetchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken)
			{
				Interlocked.Increment(ref _callCounter);
				return Task.FromResult(default(TResult));
			}

			public Task PublishAsync(IEvent @event, CancellationToken cancellationToken)
			{
				Interlocked.Increment(ref _callCounter);
				return Task.CompletedTask;
			}

			public Task<TResult> ProcessAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken)
			{
				Interlocked.Increment(ref _callCounter);
				return Task.FromResult(default(TResult));
			}
		}

		private class FakeCommand : ICommand
		{

		}
	}
}
