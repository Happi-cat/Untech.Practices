using Untech.Practices.CQRS;

namespace AsyncCommandEngine.Run.Commands
{
	public class DemoCommand : ICommand
	{
		public DelayCommand DelayCommand { get; set; }
		public ThrowCommand ThrowCommand { get; set; }
	}
}