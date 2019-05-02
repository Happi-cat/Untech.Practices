using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.AsyncCommandEngine.Metadata
{
	/// <summary>
	/// Represents <see cref="IRequestMetadata"/> that aggregates multiple instances of the <see cref="IRequestMetadata"/>.
	/// </summary>
	public class CompositeRequestMetadata : IRequestMetadata
	{
		private readonly IReadOnlyCollection<IRequestMetadata> _metadata;

		/// <summary>
		/// Initializes a new instance of the <see cref="CompositeRequestMetadata"/> with the collection of <see cref="IRequestMetadata"/>.
		/// </summary>
		/// <param name="metadata">The collection of metadatas.</param>
		/// <exception cref="ArgumentNullException"><paramref name="metadata"/> is null.</exception>
		public CompositeRequestMetadata(IEnumerable<IRequestMetadata> metadata)
		{
			if (metadata == null) throw new ArgumentNullException(nameof(metadata));

			_metadata = metadata.ToList();
		}

		/// <inheritdoc />
		public TAttr GetAttribute<TAttr>() where TAttr : Attribute
		{
			return _metadata
				.Select(n => n.GetAttribute<TAttr>())
				.FirstOrDefault(NotNull);
		}

		/// <inheritdoc />
		public IEnumerable<TAttr> GetAttributes<TAttr>() where TAttr : Attribute
		{
			return _metadata.SelectMany(n => n.GetAttributes<TAttr>());
		}

		private static bool NotNull<TAttr>(TAttr obj)
		{
			return !ReferenceEquals(obj, null);
		}
	}
}