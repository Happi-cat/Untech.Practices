using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Untech.Practices.AspNetCore.CQRS.Mappers.Accessors
{
	internal class PathGetAccessor<TProp> : IGetAccessor<HttpRequest, TProp>
	{
		private readonly string _varName;
		private readonly Parser<TProp> _parser;

		public PathGetAccessor(string varName, Parser<TProp> parser)
		{
			_varName = varName;
			_parser = parser;
		}

		public TProp Get(HttpRequest instance)
		{
			var value = instance.HttpContext.GetRouteValue(_varName);
			if (value is string textValue)
			{
				return _parser(textValue);
			}
			return (TProp)value;
		}

		public override string ToString() => $"Path({_varName})";
	}
}
