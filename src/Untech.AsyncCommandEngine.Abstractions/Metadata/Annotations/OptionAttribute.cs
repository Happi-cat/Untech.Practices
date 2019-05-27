using System;
using System.Runtime.Serialization;

namespace Untech.AsyncCommandEngine.Metadata.Annotations
{
	/// <summary>
	/// Represents attribute for custom options.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	[DataContract]
	public sealed class OptionAttribute : MetadataAttribute
	{
		private OptionAttribute()
		{

		}

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
		[DataMember]
		public string Key { get; private set; }

		/// <summary>
		/// Gets the specified option value.
		/// </summary>

		[DataMember]
		public object Value { get; private set; }
	}
}