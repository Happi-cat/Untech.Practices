using System;
using System.Threading.Tasks;

namespace Untech.AsyncJob
{
	internal partial class Orchestrator
	{
		private class Warp
		{
			private readonly object _sync = new object();

			private Task _task = Task.CompletedTask;

			public Task Task => _task;

			public bool CanRun()
			{
				return _task.Status == TaskStatus.RanToCompletion
					|| _task.Status == TaskStatus.Canceled
					|| _task.Status == TaskStatus.Faulted;
			}

			public void Run(Func<Task> function)
			{
				lock (_sync)
				{
					if (!CanRun())
						throw new InvalidOperationException("Warp is busy");

					_task = Task.Run(function);
				}
			}
		}
	}
}
