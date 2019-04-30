using System;
using System.Threading;
using System.Threading.Tasks;
using Untech.AsyncCommandEngine.Metadata.Annotations;
using Untech.Practices.CQRS.Handlers;

namespace Untech.AsyncCommandEngine.Metadata
{
	[ThrottleGroup("SomeResource")]
	public class FakeCommandHandler : ICommandHandler<FakeCommand, int>
	{
		public async Task<int> HandleAsync(FakeCommand command, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}