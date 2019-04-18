using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.AsyncCommandEngine.Metadata
{
	/// <summary>
	/// Represents <see cref="IRequestMetadataProvider"/> that aggregates multiple instances of the <see cref="IRequestMetadataProvider"/>.
	/// </summary>
	public class CompositeRequestMetadataProvider : IRequestMetadataProvider
	{
		private readonly IReadOnlyCollection<IRequestMetadataProvider> _accessors;

		/// <summary>
		/// Initializes a new instance of the <see cref="CompositeRequestMetadataProvider"/>
		/// with the collection of <see cref="IRequestMetadataProvider"/>.
		/// </summary>
		/// <param name="accessors">The collection of metadata providers.</param>
		/// <exception cref="ArgumentNullException"><paramref name="accessors"/> is null.</exception>
		public CompositeRequestMetadataProvider(IEnumerable<IRequestMetadataProvider> accessors)
		{
			if (accessors == null) throw new ArgumentNullException(nameof(accessors));

			_accessors = accessors.ToList();
		}

		/// <inheritdoc />
		public IRequestMetadata GetMetadata(string requestName)
		{
			if (string.IsNullOrEmpty(requestName)) throw new ArgumentNullException(nameof(requestName));

			var accessors = _accessors.Select(n => n.GetMetadata(requestName));

			return new CompositeRequestMetadata(accessors);
		}
	}
}