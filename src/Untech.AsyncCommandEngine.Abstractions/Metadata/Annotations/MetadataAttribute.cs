using System;
using System.Runtime.Serialization;

namespace Untech.AsyncCommandEngine.Metadata.Annotations
{
	[DataContract]
	public abstract class MetadataAttribute : Attribute
	{
		[DataMember]
		public object TypeName
		{
			get
			{
				var value = GetType().Name;
				return value.EndsWith("Attribute")
					? value.Substring(0, value.Length - "Attribute".Length)
					: value;
			}
		}
	}
}