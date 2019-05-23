﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AsyncCommandEngine.Run.Commands;
using Microsoft.Extensions.Logging;
using Serilog;
using Untech.AsyncCommandEngine;
using Untech.AsyncCommandEngine.Builder;
using Untech.AsyncCommandEngine.Features.Retrying;
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
				.ReadMetadataFrom(MetadataProviders())
				.DoSteps(Steps)
				.BuildOrchestrator(options =>
				{
					options.Warps = 10;
					options.RequestsPerWarp = 10;
					options.SlidingStep = TimeSpan.FromSeconds(1);
					options.SlidingRadius = 5;
				});

			service.StartAsync();

			Console.ReadKey();

			service.StopAsync(TimeSpan.Zero).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		private static void Steps(MiddlewareCollection steps)
		{
			steps
				.Then(builder => new DemoMiddleware(builder.GetLogger()))
				.Then(builder =>
				{
					var logger = builder.GetLogger("Metrics");
					return (ctx, next) => MetricsMiddleware(ctx, next, logger);
				})
				.ThenRetry(new RetryPolicy(new[] { typeof(TimeoutException) }))
				.ThenThrottling(options =>
				{
					options.DefaultRunAtOnceInGroup = 2;
				})
				.ThenWatchDog(options =>
				{
					options.DefaultTimeout = TimeSpan.FromSeconds(10);
				})
				.Final(builder => new CqrsStrategy(builder.GetLogger("Handlers")));
		}

		private static IEnumerable<IRequestMetadataProvider> MetadataProviders()
		{
			yield return new BuiltInRequestMetadataProvider(typeof(Program).Assembly);
			yield return new RequestMetadataProvider
			{
				{ "SomeRequest", new RequestMetadata { new ThrottleAttribute(2) } },
				new RequestMetadata { new WatchDogTimeoutAttribute(2, 0, 0) }
			};
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
			yield return new DemoTransport(new[] { new CompositeCommand(), });
			//throw
			yield return new DemoTransport(new[]
			{
				new ThrowCommand(), new ThrowCommand { Error = new TimeoutException() },
			});
			// delays
			yield return new DemoTransport(new[]
			{
				new DelayCommand(TimeSpan.FromSeconds(2)), new DelayCommand(TimeSpan.FromMinutes(2)),
				new DelayCommand(TimeSpan.FromSeconds(20))
				{
					Meta = new List<Attribute> { new WatchDogTimeoutAttribute(30) }
				},
			});
			// combined
			yield return new DemoTransport(new[]
			{
				new CompositeCommand { Delay = new DelayCommand(TimeSpan.FromSeconds(2)), },
				new CompositeCommand { Delay = new DelayCommand(TimeSpan.FromMinutes(2)), },
				new CompositeCommand
				{
					Delay = new DelayCommand(TimeSpan.FromSeconds(2)), Throw = new ThrowCommand()
				},
				new CompositeCommand
				{
					Delay = new DelayCommand(TimeSpan.FromMinutes(2)), Throw = new ThrowCommand()
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