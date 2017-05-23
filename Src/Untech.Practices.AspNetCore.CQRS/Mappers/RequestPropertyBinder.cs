using System;
using Microsoft.AspNetCore.Http;

namespace Untech.Practices.AspNetCore.CQRS.Mappers
{
	internal class RequestPropertyBinder<T, TProp> : IPropertyBinder<HttpRequest, T>
	{
		private readonly IGetAccessor<HttpRequest, TProp> _sourceAccessor;
		private readonly ISetAccessor<T, TProp> _destAccessor;

		public RequestPropertyBinder(IGetAccessor<HttpRequest, TProp> sourceAccessor, ISetAccessor<T, TProp> destAccessor)
		{
			_sourceAccessor = sourceAccessor;
			_destAccessor = destAccessor;
		}

		public void Bind(HttpRequest sourceContainer, T destContainer)
		{
			_destAccessor.Set(destContainer, _sourceAccessor.Get(sourceContainer));
		}

		public override string ToString()
		{
			return string.Format("{0} = {1}", _destAccessor, _sourceAccessor);
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			if (format == "S") return _sourceAccessor.ToString();
			if (format == "D") return _destAccessor.ToString();
			return ToString();
		}
	}
}
