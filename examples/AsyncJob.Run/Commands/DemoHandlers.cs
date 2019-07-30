using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Untech.AsyncJob.Metadata;
using Untech.AsyncJob.Metadata.Annotations;
using Untech.Practices;
using Untech.Practices.CQRS;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;

namespace AsyncJob.Run.Commands
{
	[WatchDogTimeout(0, 0, 10)]
	[ThrottleGroup("DemoHandlers")]
	public class DemoHandlers :
		ICommandHandler<CompositeCommand, None>,
		ICommandHandler<DelayCommand, None>,
		ICommandHandler<ThrowCommand, None>,
		ICommandHandler<HelloCommand, None>,
		ICommandHandler<ProduceInProcess, None>
	{
		private readonly ILogger _logger;

		public DemoHandlers(ILogger logger)
		{
			_logger = logger;
		}

		public async Task<None> HandleAsync(CompositeCommand request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Demo in progress");
			if (request.Delay != null)
			{
				await HandleAsync(request.Delay, cancellationToken);
			}

			if (request.Throw != null)
			{
				await HandleAsync(request.Throw, cancellationToken);
			}

			if (request.Hello != null)
			{
				await HandleAsync(request.Hello, cancellationToken);
			}

			_logger.LogInformation("Demo completed");
			return None.Value;
		}

		public async Task<None> HandleAsync(DelayCommand request, CancellationToken cancellationToken)
		{
			await Task.Delay(request.Timeout, cancellationToken);
			return None.Value;
		}

		public Task<None> HandleAsync(ThrowCommand request, CancellationToken cancellationToken)
		{
			throw request.GetError() ?? new InvalidOperationException("Thrown");
		}

		public Task<None> HandleAsync(HelloCommand request, CancellationToken cancellationToken)
		{
			_logger.Log(LogLevel.Information, request.Message);

			return None.AsTask;
		}

		public async Task<None> HandleAsync(ProduceInProcess request, CancellationToken cancellationToken)
		{
			await InProcess.Instance.EnqueueAsync(new HelloCommand { Message = "Immediate In Process" }, cancellationToken);
			await InProcess.Instance.EnqueueAsync(new HelloCommand { Message = "Expirable In Process" },
				cancellationToken,
				new QueueOptions { ExpiresAfter = TimeSpan.FromMinutes(1) });
			await InProcess.Instance.EnqueueAsync(new HelloCommand { Message = "Delayed In Process" },
				cancellationToken,
				new QueueOptions { ExecuteAfter = TimeSpan.FromMinutes(1) });

			return None.Value;
		}
	}
}
