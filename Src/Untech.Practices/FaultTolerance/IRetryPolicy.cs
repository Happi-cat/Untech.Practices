using System;

namespace Untech.Practices.FaultTolerance
{
	public interface IRetryPolicy
	{
		void Execute(Action action);
	}
}