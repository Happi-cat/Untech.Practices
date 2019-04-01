using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Untech.AsyncCommandEngine.Processing;

namespace Untech.AsyncCommandEngine
{
	public interface ITransport
	{
		Task<ReadOnlyCollection<Request>> GetRequestsAsync(int count);
		Task CompleteRequestAsync(Request request);
		Task FailRequestAsync(Request request, Exception exception);
	}
}