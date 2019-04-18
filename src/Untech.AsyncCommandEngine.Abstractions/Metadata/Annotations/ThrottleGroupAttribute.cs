using System;

namespace Untech.AsyncCommandEngine.Metadata.Annotations
{
	/// <summary>
	/// Sets the flag that current requests belongs to the specified throttle group.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class ThrottleGroupAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ThrottleGroupAttribute" /> with the predefined <paramref name="group"/>.
		/// </summary>
		/// <param name="group">The name of throttle group.</param>
		public ThrottleGroupAttribute(string group)
		{
			Group = @group;
		}

		/// <summary>
		/// Gets the name of throttle group.
		/// </summary>
		public string Group { get; private set; }
	}
}