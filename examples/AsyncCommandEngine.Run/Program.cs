﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Untech.AsyncCommandEngine;
using Untech.AsyncCommandEngine.Features.Throttling;
using Untech.AsyncCommandEngine.Features.WatchDog;
using Untech.AsyncCommandEngine.Metadata;
using Untech.AsyncCommandEngine.Processing;
using Untech.Practices.CQRS.Dispatching;

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

			public IEnumerable<T> ResolveMany<T>() where T : class
			{
				return Enumerable.Empty<T>();
			}

			public TypeInfo FindRequestType(string requestName)
			{
				return _types
					.Where(t => t.FullName == requestName)
					.Select(t => t.GetTypeInfo())
					.FirstOrDefault();
			}

			public object MaterializeRequest(Request request)
			{
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

			public async Task InvokeAsync(Context context, RequestProcessorCallback next)
			{
				context.TraceIdentifier = Interlocked.Increment(ref _nextTraceId).ToString();

				try
				{
					await next(context);
				}
				catch (Exception e)
				{
					Console.WriteLine("{0}:{1}: crashed with {2}", context.TraceIdentifier, DateTime.UtcNow.Ticks,
						e.Message);
					throw;
				}
			}
		}

		class DemoRequest<T> : Request
		{
			public DemoRequest(T body)
			{
				Identifier = Guid.NewGuid().ToString("B");

				var jsonBody = JsonConvert.SerializeObject(body, new JsonSerializerSettings
				{
					TypeNameHandling = TypeNameHandling.All
				});
				Body = new MemoryStream(Encoding.UTF8.GetBytes(jsonBody));

				Name = typeof(T).FullName;
				Created = DateTimeOffset.UtcNow;
			}

			public override string Identifier { get; }
			public override string Name { get; }

			public override Stream Body { get; }
			public override DateTimeOffset Created { get; }
			public override IDictionary<string, string> Attributes { get; }
		}

		static void Main(string[] args)
		{
			Test1();

			var type = typeof(DemoHandlers);

			var metadataAccessors = new BuiltInRequestMetadataAccessors(new[] { type.Assembly });

			var service = new EngineBuilder()
				.Use(() => new DemoMiddleware())
				.UseThrottling(new ThrottleOptions { DefaultRunAtOnceInGroup = 2 })
				.UseWatchDog(new WatchDogOptions
				{
					DefaultTimeout = TimeSpan.FromMinutes(1),
					RequestTimeouts = new ReadOnlyDictionary<string, TimeSpan>(new Dictionary<string, TimeSpan>
					{
						["Library1.Command1"] = TimeSpan.FromMinutes(10)
					})
				})
				.BuildService(new CqrsStrategy());


			var contexts = new[]
			{
				CreateContext(new DemoCommand(), metadataAccessors),
				CreateContext(new DemoCommand(), metadataAccessors),
				CreateContext(new DelayCommand
				{
					Timeout = TimeSpan.FromSeconds(20)
				}, metadataAccessors),
				CreateContext(new ThrowCommand(), metadataAccessors),
				CreateContext(new DemoCommand
				{
					DelayCommand = new DelayCommand { Timeout = TimeSpan.FromSeconds(10) }
				}, metadataAccessors),
				CreateContext(new DelayCommand { Timeout = TimeSpan.FromMinutes(2) }, metadataAccessors),
				CreateContext(new DemoCommand(), metadataAccessors),
				CreateContext(new DemoCommand(), metadataAccessors),
				CreateContext(new DelayCommand
				{
					Timeout = TimeSpan.FromSeconds(20)
				}, metadataAccessors),
				CreateContext(new ThrowCommand(), metadataAccessors),
				CreateContext(new DemoCommand
				{
					DelayCommand = new DelayCommand { Timeout = TimeSpan.FromSeconds(10) }
				}, metadataAccessors),
				CreateContext(new DelayCommand { Timeout = TimeSpan.FromMinutes(2) }, metadataAccessors),
			};

//			var tasks = contexts
//					.Select(ctx => (ctx, Task.Run(() => service.InvokeAsync(ctx))))
//					.ToArray();
//
//			Task.WaitAll(tasks.Select(n => n.Item2).ToArray());

			service.InvokeAsync(CreateContext(new ThrowCommand(), metadataAccessors)).GetAwaiter().GetResult();
			;
		}

		private static Context CreateContext<T>(T body, BuiltInRequestMetadataAccessors metadataAccessors)
		{
			return new Context(
				new DemoRequest<T>(body),
				metadataAccessors.GetMetadata(typeof(T).FullName)
			);
		}


		private static void Test1()
		{
			Func<int, Task> taskAction = async (int n) =>
			{
				Console.WriteLine("{0}: B in {1}", n, Thread.CurrentThread.ManagedThreadId);
				await Task.CompletedTask;
				Console.WriteLine("{0}: E in {1}", n, Thread.CurrentThread.ManagedThreadId);
			};

			Console.WriteLine("B in {0}", Thread.CurrentThread.ManagedThreadId);

			Task.WhenAll(Enumerable.Range(0, 10).Select(taskAction))
				.GetAwaiter()
				.GetResult();

			Console.WriteLine("E in {0}", Thread.CurrentThread.ManagedThreadId);
		}
	}
}