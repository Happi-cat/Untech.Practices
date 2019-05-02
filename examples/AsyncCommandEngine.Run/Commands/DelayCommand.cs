using System;
using System.Collections.Generic;
using Untech.Practices.CQRS;

namespace AsyncCommandEngine.Run.Commands
{
	public class DelayCommand : DemoCommandBase, ICommand
	{
		private DelayCommand()
		{

		}

		public DelayCommand(TimeSpan timeout)
		{
			Timeout = timeout;
		}

		public TimeSpan Timeout { get; set; }
	}
}