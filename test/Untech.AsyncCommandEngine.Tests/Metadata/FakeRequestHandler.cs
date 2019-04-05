using System;
using System.Threading;
using System.Threading.Tasks;
using Untech.Practices.CQRS.Handlers;

namespace Untech.AsyncCommandEngine.Metadata
{
	[ThrottleGroup("SomeResource")]
	public class FakeRequestHandler : ICommandHandler<FakeRequest, int>
	{
		public async Task<int> HandleAsync(FakeRequest request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}