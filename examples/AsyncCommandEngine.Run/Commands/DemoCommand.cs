using Untech.AsyncCommandEngine.Metadata;
using Untech.Practices.CQRS;

namespace AsyncCommandEngine.Run.Commands
{
	[Option("a", 1)]
	[Option("b", "1")]
	[Option("c", 1.0)]
	[Option("d", new [] { 1, 2, 3, 4 })]
	public class DemoCommand : ICommand
	{
		public DelayCommand DelayCommand { get; set; }
		public ThrowCommand ThrowCommand { get; set; }
	}
}