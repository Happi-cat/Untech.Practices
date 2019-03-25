using System;

namespace Untech.AsyncCommandEngine.Metadata
{
	/// <summary>
	/// Sets flag for current request that it can be debounced by payload
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class DebounceAttribute : Attribute
	{

	}
}