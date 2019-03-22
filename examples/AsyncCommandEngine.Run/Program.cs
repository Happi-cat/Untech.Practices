using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Untech.AsyncCommandEngine;
using Untech.AsyncCommandEngine.Features.Throttling;
using Untech.AsyncCommandEngine.Features.WatchDog;
using Untech.Practices.CQRS.Dispatching;

namespace AsyncCommandEngine.Run
{
	class Program
	{
		class DemoRequestReflection : IRequestTypeFinder, IRequestMaterializer, ITypeResolver
		{
			private readonly IReadOnlyList<Type> _types = new List<Type>
			{
				typeof(DemoCommand),
				typeof(DelayCommand),
				typeof(ThrowCommand)
			};

			public TypeInfo Find(string typeName)
			{
				return _types
					.Where(t => t.FullName == typeName)
					.Select(t => t.GetTypeInfo())
					.FirstOrDefault();
			}

			public object Materialize(Request request)
			{
				var json = new StreamReader(request.Body).ReadToEnd();

				return JsonConvert.DeserializeObject(json, new JsonSerializerSettings
				{
					TypeNameHandling = TypeNameHandling.All
				});
			}

			public T ResolveOne<T>() where T : class
			{
				return new AsyncCommandEngine.Run.DemoHandlers() as T;
			}

			public IEnumerable<T> ResolveMany<T>() where T : class
			{
				return Enumerable.Empty<T>();
			}
		}

		class DemoMiddleware : IRequestProcessorMiddleware
		{
			public async Task InvokeAsync(Context context, RequestProcessorCallback next)
			{

				try
				{
					await next(context);
				}
				catch (Exception e)
				{
					Console.WriteLine("{0}:{1}: crashed with {2}", context.TraceIdentifier, DateTime.UtcNow.Ticks, e.Message);
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
			public override IAttributesDictionary Attributes { get; }
		}

		static void Main(string[] args)
		{
			var type = typeof(DemoHandlers);

			var metadataAccessors = new RequestMetadataAccessors(new[] { type.Assembly });

			var service = new AceBuilder()
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
				.BuildService(new DemoRequestReflection(),
					new DemoRequestReflection(),
					ctx => new Dispatcher(new DemoRequestReflection()));


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


			var traceIdentifier = 1;
			foreach (var context in contexts)
			{
				context.TraceIdentifier = traceIdentifier.ToString();
				traceIdentifier++;
			}

			var tasks = contexts
					.Select(ctx => (ctx, Task.Run(() => service.InvokeAsync(ctx))))
					.ToArray();

			Task.WaitAll(tasks.Select(n => n.Item2).ToArray());
		}

		private static Context CreateContext<T>(T body, RequestMetadataAccessors metadataAccessors)
		{
			return new Context(
				new DemoRequest<T>(body),
				metadataAccessors.GetMetadata(typeof(T).FullName)
			);
		}

	}
}