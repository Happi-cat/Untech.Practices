using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using AsyncCommandEngine.Examples;
using AsyncCommandEngine.Examples.Features.Debounce;
using AsyncCommandEngine.Examples.Features.Throttling;
using Untech.AsyncCommmandEngine.Abstractions;

namespace AsyncCommandEngine.Run
{
	class Program
	{
		static void Main(string[] args)
		{
			var type = typeof(DummyCommandHandler);

			var metadata = new RequestMetadataProvider(new[] { type.Assembly })
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
				.BuildService();


			service.Execute(new AceContext(new AceRequest("test", DateTimeOffset.UtcNow, metadata)));
		}
	}
}