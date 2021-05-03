using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Untech.AsyncJob.Metadata;
using Untech.AsyncJob.Processing;
using Untech.AsyncJob.Transports;

namespace Untech.AsyncJob
{
	internal partial class Orchestrator : IOrchestrator, IDisposable
	{
		private enum State
		{
			Inactive = 0,
			Starting = 1,
			Working = 2,
			Stopping = 3,
			Stopped = 4
		};

		private readonly OrchestratorOptions _options;
		private readonly ITransport _transport;
		private readonly IRequestProcessor _requestProcessor;
		private readonly IRequestMetadataProvider _metadataProvider;
		private readonly ILogger _logger;
		private readonly object _warpUseSyncRoot = new object();

		private readonly IReadOnlyCollection<Warp> _warps;

		private State _state;
		private CancellationTokenSource _aborted;
		[CanBeNull]
		private SlidingTimer _timer;

		public Orchestrator(OrchestratorOptions options,
			ITransport transport,
			IRequestMetadataProvider metadataProvider,
			IRequestProcessor requestProcessor,
			ILogger<Orchestrator> logger)
		{
			_options = options ?? throw new ArgumentNullException(nameof(options));
			_transport = transport ?? throw new ArgumentNullException(nameof(transport));
			_requestProcessor = requestProcessor ?? throw new ArgumentNullException(nameof(requestProcessor));
			_metadataProvider = metadataProvider ?? throw new ArgumentNullException(nameof(metadataProvider));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));

			_warps = Enumerable.Range(0, options.Warps).Select(n => new Warp()).ToList();
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_state = State.Starting;
			_timer = new SlidingTimer(OnTimer, _options.SlidingStep, _options.SlidingRadius, _logger);
			_aborted = new CancellationTokenSource();

			_state = State.Working;
			return Task.CompletedTask;
		}

		public async Task StopAsync(TimeSpan delay, CancellationToken cancellationToken)
		{
			_state = State.Stopping;
			_timer?.Dispose();
			_timer = null;

			try
			{
				try
				{
					await Task.Delay(delay, cancellationToken);
				}
				catch (OperationCanceledException)
				{
					// Prevent throwing if the Delay is cancelled
				}

				_aborted?.Cancel();
			}
			finally
			{
				var executingTask = Task.WhenAll(_warps.Select(s => s.Task));

				// Wait until the task completes or the stop token triggers
				await Task.WhenAny(executingTask);
				await _transport.FlushAsync();
				_state = State.Stopped;
			}
		}

		public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
		{
			var data = new Dictionary<string, object>
			{
				["Warps"] = _warps.Count.ToString(),
				["WarpsFree"] = _warps.Count(w => w.CanRun()),
				["SlidingPercentage"] = _timer?.GetSlidingPercentage()
			};

			return Task.FromResult(HealthCheckResult.Healthy(_state.ToString(), data));
		}

		private void OnTimer()
		{
			lock (_warpUseSyncRoot)
			{
				var slot = _warps.FirstOrDefault(n => n.CanRun());

				if (slot != null)
				{
					_logger.IsFreeWarpAvailable();
					slot.Run(WarpStartAsync);
				}
				else
				{
					_logger.IsNoFreeWarpAvailable();
				}
			}
		}

		private async Task WarpStartAsync()
		{
			try
			{
				var requests = await _transport.GetRequestsAsync(_options.RequestsPerWarp);

				UpdateSlidingCoefficient(requests.Length);

				await Task.WhenAll(requests.Select(WarpExecuteAsync));
			}
			catch (Exception e)
			{
				_logger.WarpCrashed(e);
				throw;
			}

			void UpdateSlidingCoefficient(int requestCount)
			{
				if (_options.SlidingMode == SlidingMode.Default)
				{
					if (_options.RequestsPerWarp < 2) return;

					var max = _options.RequestsPerWarp;

					if (requestCount <= 0.3f * max) _timer?.Increase();
					else if (0.7f * max <= requestCount) _timer?.Decrease();
				}
				else if (_options.SlidingMode == SlidingMode.AnyAndNone)
				{
					if (requestCount == 0) _timer?.Increase();
					else _timer?.Decrease();
				}
				else
				{
					throw new NotSupportedException($"Sliding Mode {_options.SlidingMode} is not supported");
				}
			}
		}

		private Task WarpExecuteAsync(Request request)
		{
			var context = new Context(request, _metadataProvider)
			{
				Aborted = _aborted.Token
			};

			return _options.RunRequestsInWarpAllAtOnce
				? Task.Run(() => WarpExecuteAsync(context))
				: WarpExecuteAsync(context);
		}

		private async Task WarpExecuteAsync(Context context)
		{
			try
			{
				await _requestProcessor.InvokeAsync(context);
			}
			catch (Exception exception)
			{
				await _transport.FailRequestAsync(context.Request, exception);
				throw;
			}

			await _transport.CompleteRequestAsync(context.Request);
		}

		public void Dispose()
		{
			_aborted?.Cancel();
			_aborted?.Dispose();
			_aborted = null;
			_timer?.Dispose();
			_timer = null;
		}
	}
}
