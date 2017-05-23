using Microsoft.AspNetCore.Http;

namespace Untech.Practices.AspNetCore.CQRS.Mappers.Accessors
{
	internal class CookieGetAccessor<TProp> : IGetAccessor<HttpRequest, TProp>
	{
		private readonly Parser<TProp> _parser;
		private readonly string _cookieName;

		public CookieGetAccessor(string cookieName, Parser<TProp> parser)
		{
			_parser = parser;
			_cookieName = cookieName;
		}

		public TProp Get(HttpRequest instance) => _parser(instance.Cookies[_cookieName]);

		public override string ToString() => $"Cookie({_cookieName})";
	}
}
