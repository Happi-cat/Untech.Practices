using System;
using Untech.Practices.CQRS;

namespace AsyncJob.Run.Commands
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
