using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AsyncCommandEngine.Run.Commands;
using MoreLinq;
using Untech.AsyncCommandEngine;

namespace AsyncCommandEngine.Run
{
	internal class DemoTransport : ITransport
	{
		private static int s_nextIdentifier = 1;

		private readonly Random _rand = new Random();
		private readonly IReadOnlyCollection<object> _requestTemplates;

		public DemoTransport()
		{
			_requestTemplates = new List<object>
			{
				// bare
				new DemoCommand(),

				//throw
				new ThrowCommand(),

				// delays
				new DelayCommand { Timeout = TimeSpan.FromSeconds(2) },
				new DelayCommand { Timeout = TimeSpan.FromMinutes(2) },

				// combined
				new DemoCommand { DelayCommand = new DelayCommand { Timeout = TimeSpan.FromSeconds(2) }, },
				new DemoCommand { DelayCommand = new DelayCommand { Timeout = TimeSpan.FromMinutes(2) }, },
				new DemoCommand
				{
					DelayCommand = new DelayCommand { Timeout = TimeSpan.FromSeconds(2) },
					ThrowCommand = new ThrowCommand()
				},
				new DemoCommand
				{
					DelayCommand = new DelayCommand { Timeout = TimeSpan.FromMinutes(2) },
					ThrowCommand = new ThrowCommand()
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

		private static Request Create<T>(T body)
		{
			var id = Interlocked.Increment(ref s_nextIdentifier).ToString();
			return new DemoRequest(id, body);
		}
	}
}