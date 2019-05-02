using System;
using Untech.Practices.CQRS;

namespace AsyncCommandEngine.Run.Commands
{
	public class ThrowCommand : DemoCommandBase, ICommand
	{
		public Exception Error { get; set; }
	}
}