using System;

namespace Untech.AsyncCommandEngine.Metadata.Annotations
{
	/// <summary>
	/// Sets flag that current request should be throttled and cannot be run more than <see cref="RunAtOnce"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class ThrottleAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ThrottleAttribute"/> with the specified <paramref name="runAtOnce"/>.
		/// </summary>
		/// <param name="runAtOnce">The number of max concurrent requests that can be run at once.</param>
		public ThrottleAttribute(int runAtOnce)
		{
			RunAtOnce = runAtOnce;
		}

		/// <summary>
		/// Gets the number of max concurrent requests that can be run at once.
		/// </summary>
		public int RunAtOnce { get; private set; }
	}
}