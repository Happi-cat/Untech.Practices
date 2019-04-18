using System;

namespace Untech.AsyncCommandEngine.Metadata.Annotations
{
	/// <summary>
	/// Represents attribute for custom options.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class OptionAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OptionAttribute"/>
		/// with the specified <paramref name="key"/> and <paramref name="value"/>
		/// </summary>
		/// <param name="key">The option key.</param>
		/// <param name="value">The option value.</param>
		public OptionAttribute(string key, object value)
		{
			Key = key;
			Value = value;
		}

		/// <summary>
		/// Gets the specified option key.
		/// </summary>
		public string Key { get; private set; }

		/// <summary>
		/// Gets the specified option value.
		/// </summary>
		public object Value { get; private set; }
	}
}