using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Untech.AsyncCommandEngine.Metadata
{
	public class RequestMetadataProvider : IRequestMetadataProvider, IEnumerable<IRequestMetadata>
	{
		private readonly IDictionary<string, IRequestMetadata> _metadata;
		private IRequestMetadata _defaultMetadata;

		public RequestMetadataProvider() : this(NullRequestMetadata.Instance)
		{
		}

		public RequestMetadataProvider(IRequestMetadata defaultMetadata)
		{
			_metadata = new Dictionary<string, IRequestMetadata>();
			_defaultMetadata = defaultMetadata;
		}


		public RequestMetadataProvider(IEnumerable<KeyValuePair<string, IRequestMetadata>> metadata)
			:this(metadata, NullRequestMetadata.Instance)
		{
		}

		public RequestMetadataProvider(
			IEnumerable<KeyValuePair<string, IRequestMetadata>> metadata,
			IRequestMetadata defaultMetadata)
		{
			_metadata = metadata.ToDictionary(n => n.Key, n => n.Value);
			_defaultMetadata = defaultMetadata;
		}


		public IRequestMetadata GetMetadata(string requestName)
		{
			return new CompositeRequestMetadata(GetItems());

			IEnumerable<IRequestMetadata> GetItems()
			{
				if (_metadata.TryGetValue(requestName, out var metadata))
					yield return metadata;

				yield return _defaultMetadata;
			}
		}

		public RequestMetadataProvider Add(string requestName, IRequestMetadata metadata)
		{
			if (requestName == null) throw new ArgumentNullException(nameof(requestName));
			if (metadata == null) throw new ArgumentNullException(nameof(metadata));

			_metadata.Add(requestName, metadata);

			return this;
		}

		public RequestMetadataProvider Add(IRequestMetadata defaultMetadata)
		{
			if (defaultMetadata == null) throw new ArgumentNullException(nameof(defaultMetadata));

			_defaultMetadata = defaultMetadata;

			return this;
		}

		public IEnumerator<IRequestMetadata> GetEnumerator()
		{
			return _metadata
				.Select(n => n.Value)
				.Concat(new[] {_defaultMetadata})
				.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}