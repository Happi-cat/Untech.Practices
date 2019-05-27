using System;
using System.Runtime.Serialization;

namespace Untech.AsyncCommandEngine.Metadata.Annotations
{
	/// <summary>
	/// Sets flag that current request should be throttled and cannot be run more than <see cref="RunAtOnce"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	[DataContract]
	public sealed class ThrottleAttribute : MetadataAttribute
	{
		private ThrottleAttribute()
		{

		}

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
		[DataMember]
		public int RunAtOnce { get; private set; }
	}
}