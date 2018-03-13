using System;

namespace Untech.Practices.DataStorage.Cache
{
	/// <summary>
	/// Represents strongly-typed cache key in URI-like format.
	/// </summary>
	public sealed class CacheKey : IFormattable
	{
		private readonly Uri _uri;
		private readonly string _key;

		/// <summary>
		/// Initializes a new instance of the <see cref="CacheKey"/> class.
		/// </summary>
		/// <param name="root">Root segment.</param>
		/// <param name="resource">Resource segment.</param>
		/// <param name="fragment">Optional fragment.</param>
		/// <exception cref="ArgumentNullException"><paramref name="root"/> or <paramref name="resource"/> is null or empty.</exception>
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

		/// <summary>
		/// Returns string representation of current <see cref="CacheKey"/>.
		/// </summary>
		/// <returns>String representation in 'G' format.</returns>
		public override string ToString() => ToString("G", null);

		/// <summary>
		/// Returns formatted <see cref="CacheKey"/>.
		/// Supports next formats:
		/// G - "root/resource#fragment";
		/// URI or U - "cache://root/resource#fragment"
		/// </summary>
		/// <param name="format">Format from a list:
		/// G - "root/resource#fragment";
		/// URI or U - "cache://root/resource#fragment"
		/// </param>
		/// <param name="formatProvider"></param>
		/// <returns>String representation in the specified <paramref name="format"/>.</returns>
		/// <exception cref="FormatException">Format string is not supported.</exception>
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