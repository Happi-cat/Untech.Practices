using System;

namespace Untech.AsyncCommandEngine.Features.Throttling
{
	[AttributeUsage(AttributeTargets.Class)]
	public class ThrottleGroupAttribute : Attribute
	{
		public ThrottleGroupAttribute(string group)
		{
			Group = @group;
		}

		public string Group { get; }
	}
}