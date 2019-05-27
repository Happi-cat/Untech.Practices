using System;
using System.Runtime.Serialization;

namespace Untech.AsyncCommandEngine.Metadata.Annotations
{
	/// <summary>
	/// Sets flag for current request that it can be debounced.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	[DataContract]
	public sealed class DebounceAttribute : MetadataAttribute
	{
	}
}