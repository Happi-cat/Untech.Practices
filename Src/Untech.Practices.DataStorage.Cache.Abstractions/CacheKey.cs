using System;

namespace Untech.Practices.DataStorage.Cache
{
	public sealed class CacheKey : IFormattable
	{
		private readonly Uri _uri;
		private readonly string _key;

		public CacheKey(string root, string resource, string fragment = null)
		{
			if (string.IsNullOrEmpty(root)) throw new ArgumentNullException(nameof(root));
			if (string.IsNullOrEmpty(resource)) throw new ArgumentNullException(nameof(resource));

			var builder = new UriBuilder("cache", root, -1, resource)
			{
				Fragment = fragment
			};

			_uri = builder.Uri;
			_key = _uri.GetComponents(UriComponents.Host | UriComponents.Path | UriComponents.Fragment, UriFormat.SafeUnescaped);
		}

		public override string ToString() => _key;

		public string ToString(string format, IFormatProvider formatProvider)
		{
			if (string.IsNullOrWhiteSpace(format)) format = "G";

			format = format.ToUpperInvariant();

			switch (format)
			{
				case "G":
					return _key;
				case "URI":
				case "U":
					return _uri.ToString();
				default:
					throw new FormatException(String.Format("The {0} format string is not supported.", format));
			}
		}
	}
}