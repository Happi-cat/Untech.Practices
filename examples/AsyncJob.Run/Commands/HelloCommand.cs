using Untech.Practices.CQRS;

namespace AsyncJob.Run.Commands
{
	public class HelloCommand : DemoCommandBase, ICommand
	{
		public string Message { get; set; }
	}
}
