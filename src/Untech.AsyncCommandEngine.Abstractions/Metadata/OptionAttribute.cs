using System;

namespace Untech.AsyncCommandEngine.Metadata
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class OptionAttribute : Attribute
	{
		public OptionAttribute(string key, object value)
		{
			Key = key;
			Value = value;
		}

		public string Key { get; private set; }
		public object Value { get; private set; }
	}
}