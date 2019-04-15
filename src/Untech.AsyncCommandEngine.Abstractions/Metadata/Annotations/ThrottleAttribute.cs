using System;

namespace Untech.AsyncCommandEngine.Metadata.Annotations
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class ThrottleAttribute : Attribute
	{
		public ThrottleAttribute(int runAtOnce)
		{
			RunAtOnce = runAtOnce;
		}

		public int RunAtOnce { get; set; }
	}
}