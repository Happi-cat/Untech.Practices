using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Untech.AsyncCommandEngine.Metadata
{
	public class RequestMetadataProvider : IRequestMetadataProvider, IEnumerable<IRequestMetadata>
	{
		private readonly IDictionary<string, List<IRequestMetadata>> _metadata;
		private readonly IList<IRequestMetadata> _defaultMetadata;

		public RequestMetadataProvider()
		{
			_metadata = new Dictionary<string, List<IRequestMetadata>>();
			_defaultMetadata = new List<IRequestMetadata>();
		}

		public RequestMetadataProvider(IRequestMetadata defaultMetadata)
		{
			_metadata = new Dictionary<string, List<IRequestMetadata>>();
			_defaultMetadata = new List<IRequestMetadata> { defaultMetadata };
		}

		public RequestMetadataProvider(IEnumerable<KeyValuePair<string, IRequestMetadata>> metadata)
		{
			_metadata = metadata
				.GroupBy(n => n.Key, n => n.Value)
				.ToDictionary(n => n.Key, n => n.ToList());
			_defaultMetadata = new List<IRequestMetadata>();
		}

		public RequestMetadataProvider(
			IEnumerable<KeyValuePair<string, IRequestMetadata>> metadata,
			IRequestMetadata defaultMetadata)
		{
			_metadata = metadata
				.GroupBy(n => n.Key, n => n.Value)
				.ToDictionary(n => n.Key, n => n.ToList());
			_defaultMetadata = new List<IRequestMetadata>{ defaultMetadata };
		}


		public IRequestMetadata GetMetadata(string requestName)
		{
			return new CompositeRequestMetadata(GetItems());

			IEnumerable<IRequestMetadata> GetItems()
			{
				if (_metadata.TryGetValue(requestName, out var metadata))
					foreach (var meta in metadata) yield return meta;

				foreach (var meta in _defaultMetadata) yield return meta;
			}
		}

		public RequestMetadataProvider Add(string requestName, IRequestMetadata metadata)
		{
			if (requestName == null) throw new ArgumentNullException(nameof(requestName));
			if (metadata == null) throw new ArgumentNullException(nameof(metadata));

			if (!_metadata.ContainsKey(requestName))
				_metadata.Add(requestName, new List<IRequestMetadata>());
			
			_metadata[requestName].Add(metadata);

			return this;
		}

		public RequestMetadataProvider Add(IRequestMetadata defaultMetadata)
		{
			if (defaultMetadata == null) throw new ArgumentNullException(nameof(defaultMetadata));

			_defaultMetadata.Add(defaultMetadata);

			return this;
		}

		public IEnumerator<IRequestMetadata> GetEnumerator()
		{
			return _metadata
				.SelectMany(n => n.Value)
				.Concat(_defaultMetadata)
				.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}