using Microsoft.AspNetCore.Http;

namespace Untech.Practices.AspNetCore.CQRS.Mappers.Accessors
{
	internal class CookieSetAccessor<TProp> : ISetAccessor<HttpResponse, TProp>
	{
		private readonly string _cookieName;
		private readonly Formatter<TProp> _formatter;

		public CookieSetAccessor(string cookieName, Formatter<TProp> formatter)
		{
			_cookieName = cookieName;
			_formatter = formatter;
		}

		public void Set(HttpResponse instance, TProp value) => instance.Cookies.Append(_cookieName, _formatter(value));

		public override string ToString() => $"Cookie({_cookieName})";
	}
}
