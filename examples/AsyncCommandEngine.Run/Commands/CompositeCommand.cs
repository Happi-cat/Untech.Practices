using Untech.AsyncCommandEngine.Metadata.Annotations;
using Untech.Practices.CQRS;

namespace AsyncCommandEngine.Run.Commands
{
	[Option("a", 1)]
	[Option("b", "1")]
	[Option("c", 1.0)]
	[Option("d", new [] { 1, 2, 3, 4 })]
	public class CompositeCommand : DemoCommandBase, ICommand
	{
		public DelayCommand Delay { get; set; }
		public ThrowCommand Throw { get; set; }
	}
}