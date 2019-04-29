using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Untech.AsyncCommandEngine.Metadata;
using Untech.AsyncCommandEngine.Metadata.Annotations;
using Untech.Practices;
using Untech.Practices.CQRS.Handlers;

namespace AsyncCommandEngine.Run.Commands
{
	[WatchDogTimeout(0, 0, 10)]
	[ThrottleGroup("DemoHandlers")]
	public class DemoHandlers :
		ICommandHandler<CompositeCommand, Nothing>,
		ICommandHandler<DelayCommand, Nothing>,
		ICommandHandler<ThrowCommand, Nothing>
	{
		private readonly ILogger _logger;

		public DemoHandlers(ILogger logger)
		{
			_logger = logger;
		}

		public async Task<Nothing> HandleAsync(CompositeCommand request, CancellationToken cancellationToken)
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

			_logger.LogInformation("Demo completed");
			return Nothing.AtAll;
		}

		public async Task<Nothing> HandleAsync(DelayCommand request, CancellationToken cancellationToken)
		{
			await Task.Delay(request.Timeout, cancellationToken);
			return Nothing.AtAll;
		}

		public Task<Nothing> HandleAsync(ThrowCommand request, CancellationToken cancellationToken)
		{
			throw request.Error ?? new InvalidOperationException("Thrown");
		}
	}
}