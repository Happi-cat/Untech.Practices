using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Untech.AsyncCommandEngine.Metadata;
using Untech.AsyncCommandEngine.Processing;

namespace Untech.AsyncCommandEngine
{
	internal partial class Orchestrator : IOrchestrator
	{
		private readonly OrchestratorOptions _options;
		private readonly ITransport _transport;
		private readonly IRequestProcessor _requestProcessor;
		private readonly IRequestMetadataProvider _metadataProvider;
		private readonly ILogger _logger;
		private readonly object _warpUseSyncRoot = new object();

		private readonly IReadOnlyCollection<Warp> _warps;

		private CancellationTokenSource _aborted;
		private SlidingTimer _timer;

		public Orchestrator(OrchestratorOptions options,
			ITransport transport,
			IRequestMetadataProvider metadataProvider,
			IRequestProcessor requestProcessor,
			ILoggerFactory loggerFactory)
		{
			_options = options;
			_transport = transport;
			_requestProcessor = requestProcessor;
			_metadataProvider = metadataProvider;
			_logger = loggerFactory.CreateLogger<Orchestrator>();

			_warps = Enumerable.Range(0, options.Warps).Select(n => new Warp()).ToList();
		}

		public Task StartAsync()
		{
			_timer = new SlidingTimer(OnTimer, _options.SlidingStep, _options.SlidingRadius, _logger);
			_aborted = new CancellationTokenSource();

			return Task.CompletedTask;
		}

		public Task StopAsync(TimeSpan delay)
		{
			_timer.Dispose();
			if (delay != TimeSpan.Zero) _aborted.CancelAfter(delay);

			return Task.WhenAll(_warps.Select(s => s.Task));
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

				UpdateSlidingCoefficient(requests.Count);

				await Task.WhenAll(requests.Select(WarpExecuteAsync));
			}
			catch (Exception e)
			{
				_logger.WarpCrashed(e);
				throw;
			}

			void UpdateSlidingCoefficient(int requestCount)
			{
				var l = requestCount;
				var max = _options.RequestsPerWarp;

				if (l <= 0.2f * max) _timer.Increase();
				else if (l >= 0.8f * max) _timer.Decrease();
			}
		}

		private Task WarpExecuteAsync(Request request)
		{
			var context = new Context(request, _metadataProvider.GetMetadata(request.Name))
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
				return;
			}

			await _transport.CompleteRequestAsync(context.Request);
		}
	}
}