using System;
using System.Threading;
using System.Threading.Tasks;
using Untech.AsyncJob.Metadata.Annotations;
using Untech.Practices.CQRS.Handlers;

namespace Untech.AsyncJob.Metadata
{
	[ThrottleGroup("SomeResource")]
	public class FakeCommandHandler : ICommandHandler<FakeCommand, int>
	{
		public Task<int> HandleAsync(FakeCommand command, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
