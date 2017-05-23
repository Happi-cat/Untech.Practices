using System;
using Microsoft.AspNetCore.Http;

namespace Untech.Practices.AspNetCore.CQRS.Mappers.Accessors
{
	internal class BodySetAccessor<TProp> : ISetAccessor<HttpResponse, TProp>
	{
		public void Set(HttpResponse instance, TProp value)
		{
			throw new NotImplementedException();
		}

		public override string ToString() => "Body()";
	}
}
