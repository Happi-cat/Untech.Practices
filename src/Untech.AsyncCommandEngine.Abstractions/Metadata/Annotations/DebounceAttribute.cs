using System;

namespace Untech.AsyncCommandEngine.Metadata.Annotations
{
	/// <summary>
	/// Sets flag for current request that it can be debounced by payload
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class DebounceAttribute : Attribute
	{

	}
}