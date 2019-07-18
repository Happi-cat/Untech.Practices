using System;
using Untech.Practices.CQRS;

namespace AsyncJob.Run.Commands
{
	public class ThrowCommand : DemoCommandBase, ICommand
	{
		public string Type { get; set; }

		public Exception GetError()
		{
			switch (Type)
			{
				case nameof(TimeoutException): return new TimeoutException();
				default: return null;
			}
		}
	}
}
