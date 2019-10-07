using Untech.Practices.CQRS;

namespace AsyncJob.Run.Commands
{
	public class CompositeCommand : DemoCommandBase, ICommand
	{
		public DelayCommand Delay { get; set; }
		public ThrowCommand Throw { get; set; }
		public HelloCommand Hello { get; set; }
	}
}
