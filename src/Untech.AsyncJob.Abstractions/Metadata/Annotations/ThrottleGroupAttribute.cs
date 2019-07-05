using System;
using System.Runtime.Serialization;

namespace Untech.AsyncJob.Metadata.Annotations
{
	/// <summary>
	/// Sets the flag that current requests belongs to the specified throttle group.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	[DataContract]
	public sealed class ThrottleGroupAttribute : MetadataAttribute
	{
		private ThrottleGroupAttribute()
		{
		}

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
		[DataMember]
		public string Group { get; private set; }
	}
}
