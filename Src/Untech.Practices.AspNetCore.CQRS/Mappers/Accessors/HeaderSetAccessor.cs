using Microsoft.AspNetCore.Http;

namespace Untech.Practices.AspNetCore.CQRS.Mappers.Accessors
{
	internal class HeaderSetAccessor<TProp> : ISetAccessor<HttpResponse, TProp>
	{
		private readonly string _headerName;
		private readonly Formatter<TProp> _formatter;

		public HeaderSetAccessor(string headerName, Formatter<TProp> formatter)
		{
			_headerName = headerName;
			_formatter = formatter;
		}

		public void Set(HttpResponse instance, TProp value) => instance.Headers.Append(_headerName, _formatter(value));

		public override string ToString() => $"Header({_headerName})";
	}
}
