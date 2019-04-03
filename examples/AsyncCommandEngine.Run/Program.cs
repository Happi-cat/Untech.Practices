﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MoreLinq;
using Newtonsoft.Json;
using Serilog;
using Untech.AsyncCommandEngine;
using Untech.AsyncCommandEngine.Features.Throttling;
using Untech.AsyncCommandEngine.Features.WatchDog;
using Untech.AsyncCommandEngine.Metadata;
using Untech.AsyncCommandEngine.Processing;
using Untech.Practices.CQRS.Dispatching;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace AsyncCommandEngine.Run
{
	class Program
	{
		class CqrsStrategy : ICqrsStrategy, ITypeResolver
		{
			private readonly IReadOnlyList<Type> _types = new List<Type>
			{
				typeof(DemoCommand),
				typeof(DelayCommand),
				typeof(ThrowCommand)
			};

			public T ResolveOne<T>() where T : class
			{
				return new AsyncCommandEngine.Run.DemoHandlers() as T;
			}

			public ReadOnlyCollection<T> ResolveMany<T>() where T : class
			{
				return Enumerable.Empty<T>().ToList().AsReadOnly();
			}

			public Type FindRequestType(string requestName)
			{
				return _types.First(t => t.FullName == requestName);
			}

			public object MaterializeRequest(Request request)
			{
				request.Body.Seek(0, SeekOrigin.Begin);
				var json = new StreamReader(request.Body).ReadToEnd();

				return JsonConvert.DeserializeObject(json, new JsonSerializerSettings
				{
					TypeNameHandling = TypeNameHandling.All
				});
			}

			public IDispatcher GetDispatcher(Context context)
			{
				return new Dispatcher(this);
			}
		}

		class DemoMiddleware : IRequestProcessorMiddleware
		{
			private int _nextTraceId = 0;

			private readonly ILogger _logger;

			public DemoMiddleware(ILoggerFactory loggerFactory)
			{
				_logger = loggerFactory.CreateLogger<DemoMiddleware>();
			}

			public async Task InvokeAsync(Context context, RequestProcessorCallback next)
			{
				context.TraceIdentifier = Interlocked.Increment(ref _nextTraceId).ToString();

				var reader = new StreamReader(context.Request.Body);

				_logger.Log(LogLevel.Debug, "{0} starting: {1}",
					context.TraceIdentifier,
					reader.ReadToEnd());
				try
				{
					await next(context);
				}
				catch (Exception e)
				{
					_logger.Log(LogLevel.Error,e, "{0} crashed: {1}", context.TraceIdentifier, e.Message);
				}
			}
		}

		class DemoRequest<T> : Request
		{
			private byte[] _body;

			public DemoRequest(string id, T body)
			{
				Identifier = id;

				var jsonBody = JsonConvert.SerializeObject(body, new JsonSerializerSettings
				{
					TypeNameHandling = TypeNameHandling.All
				});
				_body = Encoding.UTF8.GetBytes(jsonBody);

				Name = typeof(T).FullName;
				Created = DateTimeOffset.UtcNow;
			}

			public override string Identifier { get; }
			public override string Name { get; }

			public override Stream Body => new MemoryStream(_body);
			public override DateTimeOffset Created { get; }
			public override IDictionary<string, string> Attributes { get; }
		}


		class DemoTransport : ITransport
		{
			private int _maxHandle = 1;
			private readonly Random _rand = new Random();
			private readonly IReadOnlyCollection<Request> _demo;

			public DemoTransport(IRequestMetadataProvider metadataProvider)
			{
				_demo = new List<Request>
				{
					// bare
					Create(new DemoCommand()),

					//throw
					Create(new ThrowCommand()),

					// delays
					Create(new DelayCommand { Timeout = TimeSpan.FromSeconds(2) }),
					Create(new DelayCommand { Timeout = TimeSpan.FromMinutes(2) }),

					// combined
					Create(new DemoCommand
					{
						DelayCommand = new DelayCommand { Timeout = TimeSpan.FromSeconds(2) },
					}),
					Create(new DemoCommand
					{
						DelayCommand = new DelayCommand { Timeout = TimeSpan.FromMinutes(2) },
					}),
					Create(new DemoCommand
					{
						DelayCommand = new DelayCommand { Timeout = TimeSpan.FromSeconds(2) },
						ThrowCommand = new ThrowCommand()
					}),
					Create(new DemoCommand
					{
						DelayCommand = new DelayCommand { Timeout = TimeSpan.FromMinutes(2) },
						ThrowCommand = new ThrowCommand()
					}),
				};
			}

			public Task<ReadOnlyCollection<Request>> GetRequestsAsync(int count)
			{
				count = _rand.Next(count);
				var requests = _demo.Shuffle(_rand).Repeat().Take(count).ToList().AsReadOnly();
				return Task.FromResult(requests);
			}

			public Task CompleteRequestAsync(Request request)
			{
				return Task.CompletedTask;
			}

			public Task FailRequestAsync(Request request, Exception exception)
			{
				return Task.CompletedTask;
			}

			private Request Create<T>(T body)
			{
				var id = Interlocked.Increment(ref _maxHandle).ToString();
				return new DemoRequest<T>(id, body);
			}
		}

		static void Main(string[] args)
		{
			var logger = new LoggerConfiguration()
				.Enrich.FromLogContext()
				.MinimumLevel.Debug()
				.WriteTo.ColoredConsole(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3} {Properties} {Message}{NewLine}{Exception}")
				.CreateLogger();

			var loggerFactory = new LoggerFactory()
				.AddSerilog(logger);

			var type = typeof(DemoHandlers);

			var metadataAccessors = new BuiltInRequestMetadataProvider(new[] { type.Assembly });

			var service = new EngineBuilder()
				.UseLogger(loggerFactory)
				.UseTransport(new DemoTransport(metadataAccessors))
				.Use(() => new DemoMiddleware(loggerFactory))
				.UseThrottling(new ThrottleOptions { DefaultRunAtOnceInGroup = 2 })
				.UseWatchDog(new WatchDogOptions
				{
					DefaultTimeout = TimeSpan.FromMinutes(1),
					RequestTimeouts = new ReadOnlyDictionary<string, TimeSpan>(new Dictionary<string, TimeSpan>
					{
						["Library1.Command1"] = TimeSpan.FromMinutes(10)
					})
				})
				.BuildOrchestrator(new CqrsStrategy(), new OrchestratorOptions
				{
					Warps = 10,
					RequestsPerWarp = 10,
					SlidingStep = TimeSpan.FromSeconds(1),
					SlidingRadius = 5
				});

//			var tasks = contexts
//					.Select(ctx => (ctx, Task.Run(() => service.InvokeAsync(ctx))))
//					.ToArray();
//
//			Task.WaitAll(tasks.Select(n => n.Item2).ToArray());

			var task = Task.Run(service.StartAsync);

			Console.ReadKey();

			service.StopAsync(TimeSpan.Zero).ConfigureAwait(false).GetAwaiter().GetResult();
		}


	}
}