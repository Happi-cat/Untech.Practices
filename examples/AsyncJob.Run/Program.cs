using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AsyncJob.Run.Commands;
using Microsoft.Extensions.Logging;
using Serilog;
using Untech.AsyncJob;
using Untech.AsyncJob.Builder;
using Untech.AsyncJob.Features.CQRS;
using Untech.AsyncJob.Features.Retrying;
using Untech.AsyncJob.Features.Throttling;
using Untech.AsyncJob.Features.WatchDog;
using Untech.AsyncJob.Formatting.Json;
using Untech.AsyncJob.Metadata;
using Untech.AsyncJob.Metadata.Annotations;
using Untech.AsyncJob.Processing;
using Untech.AsyncJob.Transports;
using Untech.AsyncJob.Transports.InProcess;
using Untech.AsyncJob.Transports.Scheduled;
using Untech.Practices.CQRS.Dispatching;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace AsyncJob.Run
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			var service = new EngineBuilder(options =>
				{
					options.Warps = 10;
					options.RequestsPerWarp = 10;
					options.SlidingStep = TimeSpan.FromSeconds(1);
					options.SlidingRadius = 5;
				})
				.LogTo(Logger)
				.ReceiveRequestsFromMultiple(Transports)
				.ReadMetadataFromMultiple(MetadataProviders)
				.Do(Steps)
				.BuildOrchestrator();

			service.StartAsync();

			Console.ReadKey();

			service.StopAsync(TimeSpan.Zero).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		private static void Steps(PipelineBuilder steps)
		{
			steps
				.Add(builder => new DemoMiddleware(builder.GetLogger()))
				.Add(builder =>
				{
					var logger = builder.GetLogger("Metrics");
					return (ctx, next) => MetricsMiddleware(ctx, next, logger);
				})
				.AddRetry(new RetryPolicy(new[] { typeof(TimeoutException) }))
				.AddThrottling(options =>
				{
					options.DefaultRunAtOnceInGroup = 2;
				})
				.AddWatchDog(options =>
				{
					options.DefaultTimeout = TimeSpan.FromSeconds(10);
				})
				.Final(builder => new CqrsStrategy(new BuiltInRequestTypeFinder(typeof(Program).Assembly))
				{
					Dispatcher = new Dispatcher(new DemoServiceProvider(builder.GetLogger("Handlers"))),
					Formatter = JsonRequestContentFormatter.Default
				});
		}

		private static IEnumerable<IRequestMetadataProvider> MetadataProviders(IServiceProvider services)
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
					"[{Timestamp:HH:mm:ss} {Level:u3}] {Properties} {Message}{NewLine}")
				.CreateLogger();

			return new LoggerFactory().AddSerilog(logger);
		}

		private static IEnumerable<ITransport> Transports(IServiceProvider services)
		{
			// bare
			yield return new DemoTransport(new[] { new CompositeCommand() });
			//throw
			yield return new DemoTransport(new[]
			{
				new ThrowCommand(), new ThrowCommand { Type = nameof(TimeoutException) }
			});
			// delays
			yield return new DemoTransport(new[]
			{
				new DelayCommand(TimeSpan.FromSeconds(2)), new DelayCommand(TimeSpan.FromMinutes(2)),
				new DelayCommand(TimeSpan.FromSeconds(20))
				{
					WatchDogTimeout = TimeSpan.FromSeconds(30)
				}
			});
			// combined
			yield return new DemoTransport(new[]
			{
				new CompositeCommand { Delay = new DelayCommand(TimeSpan.FromSeconds(2)) },
				new CompositeCommand { Delay = new DelayCommand(TimeSpan.FromMinutes(2)) },
				new CompositeCommand
				{
					Delay = new DelayCommand(TimeSpan.FromSeconds(2)), Throw = new ThrowCommand()
				},
				new CompositeCommand
				{
					Delay = new DelayCommand(TimeSpan.FromMinutes(2)), Throw = new ThrowCommand()
				}
			});
			// scheduled
			yield return new ScheduledTransport(new InMemoryScheduledJobStore(new[]
			{
				ScheduledJobDefinition.Create("* * * * *", new HelloCommand { Message = "Hi (every min)" }),
				ScheduledJobDefinition.Create("*/2 * * * *", new ProduceInProcess()),
				new ScheduledJobDefinition
				{
					Cron = "*/5 * * * *",
					Name = typeof(HelloCommand).FullName,
					Content = "{\"Message\":\"How are you (every 5 min)\"}",
					ContentType = "json"
				}
			}));
			// in process
			yield return InProcess.Instance;
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

	internal static class InProcess
	{
		public static readonly InProcessTransport Instance = new InProcessTransport();
	}
}
