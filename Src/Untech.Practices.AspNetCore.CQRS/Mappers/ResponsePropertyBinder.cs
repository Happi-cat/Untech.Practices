using System;
using Microsoft.AspNetCore.Http;

namespace Untech.Practices.AspNetCore.CQRS.Mappers
{
	internal class ResponsePropertyBinder<T, TProp> : IPropertyBinder<T, HttpResponse>
	{
		private readonly IGetAccessor<T, TProp> _sourceAccessor;
		private readonly ISetAccessor<HttpResponse, TProp> _destAccessor;

		public ResponsePropertyBinder(IGetAccessor<T, TProp> sourceAccessor, ISetAccessor<HttpResponse, TProp> destAccessor)
		{
			_sourceAccessor = sourceAccessor;
			_destAccessor = destAccessor;
		}

		public void Bind(T sourceContainer, HttpResponse destContainer)
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
