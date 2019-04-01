using System;

namespace Untech.AsyncCommandEngine.Metadata
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class ThrottleGroupAttribute : Attribute
	{
		public ThrottleGroupAttribute(string group)
		{
			Group = @group;
		}

		public string Group { get; }
	}
}