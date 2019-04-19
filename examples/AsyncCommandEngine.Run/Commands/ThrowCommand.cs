using Untech.AsyncCommandEngine.Metadata.Annotations;
using Untech.Practices.CQRS;

namespace AsyncCommandEngine.Run.Commands
{
	[Debounce]
	public class ThrowCommand : DemoCommandBase, ICommand
	{

	}
}