﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AsyncCommandEngine.Run.Commands;
using Microsoft.Extensions.Logging;
using Serilog;
using Untech.AsyncCommandEngine;
using Untech.AsyncCommandEngine.Features.Throttling;
using Untech.AsyncCommandEngine.Features.WatchDog;
using Untech.AsyncCommandEngine.Metadata;
using Untech.AsyncCommandEngine.Metadata.Annotations;
using Untech.AsyncCommandEngine.Processing;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace AsyncCommandEngine.Run
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			var service = new EngineBuilder()
				.LogTo(Logger())
				.ReceiveRequestsFrom(Transports())
				.ReadMetadataFrom(
					new BuiltInRequestMetadataProvider(new[] { typeof(Program).Assembly })
				)
				.Then(builder => new DemoMiddleware(builder.GetLogger()))
				.Then(builder =>
				{
					var logger = builder.GetLogger("Metrics");
					return (ctx, next) => MetricsMiddleware(ctx, next, logger);
				})
				.ThenThrottling(new ThrottleOptions { DefaultRunAtOnceInGroup = 2 })
				.ThenWatchDog(new WatchDogOptions { DefaultTimeout = TimeSpan.FromSeconds(10) })
				.BuildOrchestrator(
					builder => new CqrsStrategy(builder.GetLogger("Handlers")),
					new OrchestratorOptions
					{
						Warps = 10, RequestsPerWarp = 10, SlidingStep = TimeSpan.FromSeconds(1), SlidingRadius = 5
					});

			service.StartAsync();

			Console.ReadKey();

			service.StopAsync(TimeSpan.Zero).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		private static ILoggerFactory Logger()
		{
			var logger = new LoggerConfiguration()
				.Enrich.FromLogContext()
				.MinimumLevel.Debug()
				.WriteTo.ColoredConsole(
					outputTemplate:
					"{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3} {Properties} {Message}{NewLine}")
				.CreateLogger();

			return new LoggerFactory().AddSerilog(logger);
		}

		private static IEnumerable<DemoTransport> Transports()
		{
			// bare
			yield return new DemoTransport(new[]
			{
				new CompositeCommand(),
			});
			//throw
			yield return new DemoTransport(new[]
			{
				new ThrowCommand(),
			});
			// delays
			yield return new DemoTransport(new[]
			{
				new DelayCommand(TimeSpan.FromSeconds(2)), new DelayCommand(TimeSpan.FromMinutes(2)),
				new DelayCommand(TimeSpan.FromSeconds(20))
				{
					AttachedMetadata = new List<Attribute> { new WatchDogTimeoutAttribute(30) }
				},
			});
			// combined
			yield return new DemoTransport(new[]
			{
				new CompositeCommand { DelayCommand = new DelayCommand(TimeSpan.FromSeconds(2)), },
				new CompositeCommand { DelayCommand = new DelayCommand(TimeSpan.FromMinutes(2)), },
				new CompositeCommand
				{
					DelayCommand = new DelayCommand(TimeSpan.FromSeconds(2)), ThrowCommand = new ThrowCommand()
				},
				new CompositeCommand
				{
					DelayCommand = new DelayCommand(TimeSpan.FromMinutes(2)), ThrowCommand = new ThrowCommand()
				}
			});
		}

		private static async Task MetricsMiddleware(Context ctx, RequestProcessorCallback next, ILogger logger)
		{
			var sw = new Stopwatch();
			sw.Start();
			try
			{
				await next(ctx);
			}
			finally
			{
				sw.Stop();
				logger.LogInformation("Elapsed: {0}", sw.Elapsed);
			}
		}
	}
}