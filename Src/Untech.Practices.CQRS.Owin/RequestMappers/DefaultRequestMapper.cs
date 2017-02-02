using System;
using Microsoft.Owin;

namespace Untech.Practices.CQRS.Owin.RequestMappers
{
	[AttributeUsage(AttributeTargets.Property)]
	public class OwinRequestKeyAtrribute : Attribute
	{
		public string Key { get; private set; }

		public OwinRequestKeyAtrribute(string key)
		{
			Key = key;
		}
	}

	[AttributeUsage(AttributeTargets.Property)]
	public class OwinRequestBodyAtrribute : Attribute
	{
	}



	class DefaultRequestMapper<T> : IRequestMapper<T>
	{
		public T Map(IOwinRequest request)
		{
			throw new NotImplementedException();
		}
	}
}