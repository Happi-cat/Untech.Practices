using System;
using Microsoft.AspNetCore.Http;

namespace Untech.Practices.AspNetCore.CQRS.Mappers.Accessors
{
	internal class BodyGetAccessor<TProp> : IGetAccessor<HttpRequest, TProp>
	{
		public TProp Get(HttpRequest instance)
		{
			throw new NotImplementedException();
		}

		public override string ToString() => "Body()";
	}
}
