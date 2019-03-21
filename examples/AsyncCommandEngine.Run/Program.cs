using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Untech.AsyncCommandEngine;
using Untech.AsyncCommandEngine.Features.WatchDog;
using Untech.Practices.CQRS.Dispatching;

namespace AsyncCommandEngine.Run
{
	class Program
	{
		class DemoRequestReflection : IRequestTypeFinder, IRequestMaterializer, ITypeResolver
		{
			public TypeInfo Find(string typeName)
			{
				return typeof(DummyCommand).GetTypeInfo();
			}

			public object Materialize(Request request)
			{
				return new DummyCommand();
			}

			public T ResolveOne<T>() where T : class
			{
				return new DummyCommandHandler() as T;
			}

			public IEnumerable<T> ResolveMany<T>() where T : class
			{
				return Enumerable.Empty<T>();
			}
		}

		class DemoRequest : Request
		{
			public override string Identifier { get; }
			public override string Name { get; }= typeof(DummyCommand).FullName;

			public override Stream Body { get; }
			public override DateTimeOffset Created { get; }
			public override IAttributesDictionary Attributes { get; }
		}

		static void Main(string[] args)
		{
			var type = typeof(DummyCommandHandler);

			var metadata = new RequestMetadataAccessors(new[] { type.Assembly })
				.GetMetadata(typeof(DummyCommand).FullName);

			var service = new AceBuilder()
//				.UseDebounce(new DebounceOptions())
//				.UseThottling(new ThrottleOptions())
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


			service.ExecuteAsync(new Context(new DemoRequest(), metadata)).GetAwaiter().GetResult();
		}
	}
}