using Microsoft.AspNetCore.Http;

namespace Untech.Practices.AspNetCore.CQRS.Mappers.Accessors
{
	internal class QueryGetAccessor<TProp> : IGetAccessor<HttpRequest, TProp>
	{
		private readonly string _argName;
		private readonly Parser<TProp> _parser;

		public QueryGetAccessor(string argName, Parser<TProp> parser)
		{
			_argName = argName;
			_parser = parser;
		}

		public TProp Get(HttpRequest instance) => _parser(instance.Query[_argName]);

		public override string ToString() => $"Query({_argName})";
	}
}
