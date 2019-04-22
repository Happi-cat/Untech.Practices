﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using AsyncCommandEngine.Run.Commands;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Untech.AsyncCommandEngine;
using Untech.AsyncCommandEngine.Features.Throttling;
using Untech.AsyncCommandEngine.Features.WatchDog;
using Untech.AsyncCommandEngine.Metadata;

namespace AsyncCommandEngine.Run
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			var loggerFactory = CreateLogger();

			var service = new EngineBuilder()
				.UseLogger(loggerFactory)
				.UseTransport(new DemoTransport())
				.Use(ctx => new DemoMiddleware(ctx.GetLogger()))
				.Use(builder =>
				{
					var logger = builder.GetLogger().CreateLogger("Metrics");
					return async (ctx, next) =>
					{
						var sw = new Stopwatch();
						sw.Start();
						try { await next(ctx); }
						finally
						{
							sw.Stop();
							logger.LogInformation("Elapsed: {0}", sw.Elapsed);
						}
					};
				})
				.UseThrottling(new ThrottleOptions { DefaultRunAtOnceInGroup = 2 })
				.UseWatchDog(new WatchDogOptions { DefaultTimeout = TimeSpan.FromSeconds(10) })
				.BuildOrchestrator(new CqrsStrategy(loggerFactory.CreateLogger("Handlers")),
					new OrchestratorOptions
					{
						Warps = 10,
						RequestsPerWarp = 10,
						SlidingStep = TimeSpan.FromSeconds(1),
						SlidingRadius = 5
					});

			service.StartAsync();

			Console.ReadKey();

			service.StopAsync(TimeSpan.Zero).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		private static ILoggerFactory CreateLogger()
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
	}
}