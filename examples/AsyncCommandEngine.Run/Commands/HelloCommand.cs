using Untech.Practices.CQRS;

namespace AsyncCommandEngine.Run.Commands
{
	public class HelloCommand : DemoCommandBase, ICommand
	{
		public string Message { get; set; }
	}
}