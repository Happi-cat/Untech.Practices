using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Untech.AsyncJob.Transports
{
	/// <summary>
	/// Represents <see cref="ITransport"/> that aggregates multiple transport behind and
	/// uses round-robin logic inside <see cref="GetRequestsAsync"/> for transport selection
	/// </summary>
	public class CompositeTransport : ITransport
	{
		private readonly IReadOnlyList<ITransport> _transports;
		private int _nextIndex;

		/// <summary>
		/// Initializes a new instance of the <see cref="CompositeTransport"/> with the specified <paramref name="transports"/>.
		/// </summary>
		/// <param name="transports">The collection of transports to aggregate.</param>
		/// <exception cref="ArgumentNullException"><paramref name="transports"/> is null.</exception>
		public CompositeTransport(IEnumerable<ITransport> transports)
		{
			if (transports == null) throw new ArgumentNullException(nameof(transports));

			_transports = transports.ToList();
		}


		/// <inheritdoc />
		public async Task<Request[]> GetRequestsAsync(int count)
		{
			foreach (var transport in GetTransportsAsRoundRobin())
			{
				var requests = await transport.GetRequestsAsync(count);
				if (requests.Length <= 0) continue;

				foreach (var request in requests) request.Items[this] = transport;
				return requests;
			}

			return new Request[0];
		}

		/// <inheritdoc />
		public async Task CompleteRequestAsync(Request request)
		{
			await GetUnderlyingTransport(request).CompleteRequestAsync(request);
		}

		/// <inheritdoc />
		public async Task FailRequestAsync(Request request, Exception exception)
		{
			await GetUnderlyingTransport(request).FailRequestAsync(request, exception);
		}

		private ITransport GetUnderlyingTransport(Request request)
		{
			return (ITransport)request.Items[this];
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
