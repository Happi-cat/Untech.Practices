using Microsoft.AspNetCore.Http;

namespace Untech.Practices.AspNetCore.CQRS.Mappers.Accessors
{
	internal class HeaderGetAccessor<TProp> : IGetAccessor<HttpRequest, TProp>
	{
		private readonly Parser<TProp> _parser;
		private readonly string _headerName;

		public HeaderGetAccessor(string headerName, Parser<TProp> parser)
		{
			_parser = parser;
			_headerName = headerName;
		}

		public TProp Get(HttpRequest instance) => _parser(instance.Headers[_headerName]);

		public override string ToString() => $"Header({_headerName})";
	}
}
