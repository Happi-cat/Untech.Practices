using System;
using Untech.Practices.CQRS.Owin.RequestExecutors;

namespace Untech.Practices.CQRS.Owin.Routing
{
	public sealed class RouteInfo : IEquatable<RouteInfo>
	{
		public string UrlTemplate { get; set; }

		public string Method { get; set; }

		public IRequestExecutor Executor { get; set; }

		public bool Equals(RouteInfo other)
		{
			if (other == null) return false;
			if (ReferenceEquals(this, other)) return true;

			return Equals(UrlTemplate, other.UrlTemplate) &&
				   Equals(Method, other.Method);
		}
	}
}