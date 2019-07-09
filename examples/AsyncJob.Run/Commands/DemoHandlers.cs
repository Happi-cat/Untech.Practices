﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Untech.AsyncJob.Metadata;
using Untech.AsyncJob.Metadata.Annotations;
using Untech.Practices;
using Untech.Practices.CQRS.Handlers;

namespace AsyncJob.Run.Commands
{
	[WatchDogTimeout(0, 0, 10)]
	[ThrottleGroup("DemoHandlers")]
	public class DemoHandlers :
		ICommandHandler<CompositeCommand, Nothing>,
		ICommandHandler<DelayCommand, Nothing>,
		ICommandHandler<ThrowCommand, Nothing>,
		ICommandHandler<HelloCommand, Nothing>
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

			if (request.Hello != null)
			{
				await HandleAsync(request.Hello, cancellationToken);
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
			throw request.GetError() ?? new InvalidOperationException("Thrown");
		}

		public async Task<Nothing> HandleAsync(HelloCommand request, CancellationToken cancellationToken)
		{
			_logger.Log(LogLevel.Information, request.Message);

			return Nothing.AtAll;
		}
	}
}