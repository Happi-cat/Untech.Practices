using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AsyncCommandEngine.Run.Commands;
using MoreLinq;
using Untech.AsyncCommandEngine;
using Untech.AsyncCommandEngine.Metadata.Annotations;
using Untech.AsyncCommandEngine.Transports;

namespace AsyncCommandEngine.Run
{
	internal class DemoTransport : ITransport
	{
		private static int s_nextIdentifier = 1;

		private readonly Random _rand = new Random();
		private readonly IReadOnlyCollection<DemoCommandBase> _requestTemplates;

		public DemoTransport()
		{
			_requestTemplates = new List<DemoCommandBase>
			{
				// bare
				new CompositeCommand(),

				//throw
				new ThrowCommand(),

				// delays
				new DelayCommand(TimeSpan.FromSeconds(2)),
				new DelayCommand(TimeSpan.FromMinutes(2)),
				new DelayCommand(TimeSpan.FromSeconds(20))
				{
					AttachedMetadata = new List<Attribute> { new WatchDogTimeoutAttribute(30) }
				},

				// combined
				new CompositeCommand { DelayCommand = new DelayCommand(TimeSpan.FromSeconds(2)), },
				new CompositeCommand { DelayCommand = new DelayCommand(TimeSpan.FromMinutes(2)), },
				new CompositeCommand
				{
					DelayCommand = new DelayCommand(TimeSpan.FromSeconds(2)), ThrowCommand = new ThrowCommand()
				},
				new CompositeCommand
				{
					DelayCommand = new DelayCommand(TimeSpan.FromMinutes(2)), ThrowCommand = new ThrowCommand()
				},
			};
		}

		public Task<ReadOnlyCollection<Request>> GetRequestsAsync(int count)
		{
			count = _rand.Next(count);
			var requests = _requestTemplates
				.Repeat()
				.Take(count)
				.Shuffle(_rand)
				.Select(Create)
				.ToList()
				.AsReadOnly();

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

		private static Request Create(DemoCommandBase body)
		{
			var id = Interlocked.Increment(ref s_nextIdentifier).ToString();
			return new DemoRequest(id, body) { AttachedMetadata = body.AttachedMetadata?.AsReadOnly() };
		}
	}
}