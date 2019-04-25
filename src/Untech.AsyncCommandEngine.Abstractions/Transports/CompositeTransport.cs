using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Untech.AsyncCommandEngine.Transports
{
	public class CompositeTransport : ITransport
	{
		private static readonly ReadOnlyCollection<Request> s_emptyRequestCollection = new List<Request>().AsReadOnly();

		private readonly IReadOnlyList<ITransport> _transports;
		private int _nextIndex;

		public CompositeTransport(IEnumerable<ITransport> transports)
		{
			if (transports == null) throw new ArgumentNullException(nameof(transports));

			_transports = transports.ToList();
		}

		public async Task<ReadOnlyCollection<Request>> GetRequestsAsync(int count)
		{
			foreach (var transport in GetTransportsAsRoundRobin())
			{
				var requests = await transport.GetRequestsAsync(count);
				if (requests.Count <= 0) continue;

				foreach (var request in requests) request.Items[this] = transport;
				return requests;
			}

			return s_emptyRequestCollection;
		}

		public async Task CompleteRequestAsync(Request request)
		{
			await ((ITransport)request.Items[this]).CompleteRequestAsync(request);
		}

		public async Task FailRequestAsync(Request request, Exception exception)
		{
			await ((ITransport)request.Items[this]).FailRequestAsync(request, exception);
		}

		private IEnumerable<ITransport> GetTransportsAsRoundRobin()
		{
			int remainingAttempts = _transports.Count;
			while (remainingAttempts-- > 0)
			{
				var index = Interlocked.Increment(ref _nextIndex);
				yield return _transports[index % _transports.Count];
			}
		}
	}
}