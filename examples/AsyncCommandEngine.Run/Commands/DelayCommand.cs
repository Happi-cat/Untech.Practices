using System;
using Untech.Practices.CQRS;

namespace AsyncCommandEngine.Run.Commands
{
	public class DelayCommand : ICommand
	{
		public TimeSpan Timeout { get; set; }
	}
}