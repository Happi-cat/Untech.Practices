using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.AsyncJob.Features.CQRS
{
	public class RequestTypeFinder : IRequestTypeFinder
	{
		private readonly IReadOnlyDictionary<string, Type> _types;

		public RequestTypeFinder(IEnumerable<Type> types)
		{
			if (types == null) throw new ArgumentNullException();

			_types = types.ToDictionary(t => t.FullName, t => t);
		}

		public Type FindRequestType(string requestName)
		{
			return _types.TryGetValue(requestName, out var type) ? type : null;
		}
	}
}