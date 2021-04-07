using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AsyncJob.Run.Commands;
using MoreLinq;
using Untech.AsyncJob;
using Untech.AsyncJob.Transports;

namespace AsyncJob.Run
{
	internal class DemoTransport : ITransport
	{
		private static int s_nextIdentifier = 1;

		private readonly Random _rand = new Random();
		private readonly IReadOnlyCollection<DemoCommandBase> _requestTemplates;

		public DemoTransport(IEnumerable<DemoCommandBase> commands)
		{
			_requestTemplates = commands.ToList();
		}

		public Task<Request[]> GetRequestsAsync(int count)
		{
			count = _rand.Next(count);
			var requests = _requestTemplates
				.Repeat()
				.Take(count)
				.Shuffle(_rand)
				.Select(Create)
				.ToArray();

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

		public Task FlushAsync()
		{
			return Task.CompletedTask;
		}

		private static Request Create(DemoCommandBase body)
		{
			var id = Interlocked.Increment(ref s_nextIdentifier).ToString();
			return new DemoRequest(id, body) { AttachedMetadata = body.GetMetadata().ToList().AsReadOnly() };
		}
	}
}
