using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.AsyncJob.Metadata
{
	/// <summary>
	/// Represents <see cref="IRequestMetadataProvider"/> that aggregates multiple instances of the <see cref="IRequestMetadataProvider"/>.
	/// </summary>
	public class CompositeRequestMetadataProvider : IRequestMetadataProvider
	{
		private readonly IReadOnlyCollection<IRequestMetadataProvider> _providers;

		/// <summary>
		/// Initializes a new instance of the <see cref="CompositeRequestMetadataProvider"/>
		/// with the collection of <see cref="IRequestMetadataProvider"/>.
		/// </summary>
		/// <param name="providers">The collection of metadata providers.</param>
		/// <exception cref="ArgumentNullException"><paramref name="providers"/> is null.</exception>
		public CompositeRequestMetadataProvider(IEnumerable<IRequestMetadataProvider> providers)
		{
			if (providers == null) throw new ArgumentNullException(nameof(providers));

			_providers = providers.ToList();
		}

		/// <inheritdoc />
		public IRequestMetadata GetMetadata(string requestName)
		{
			if (string.IsNullOrEmpty(requestName)) throw new ArgumentNullException(nameof(requestName));

			var accessors = _providers.Select(n => n.GetMetadata(requestName));

			return new CompositeRequestMetadata(accessors);
		}
	}
}
