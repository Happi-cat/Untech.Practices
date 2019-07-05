using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Untech.AsyncJob.Metadata
{
	public class RequestMetadataProvider : IRequestMetadataProvider, IEnumerable<IRequestMetadata>
	{
		public readonly string DefaultMetadataKey = "*";

		private readonly IDictionary<string, List<IRequestMetadata>> _metadata;

		public RequestMetadataProvider()
		{
			_metadata = new Dictionary<string, List<IRequestMetadata>>();
		}

		public RequestMetadataProvider(IEnumerable<KeyValuePair<string, IRequestMetadata>> metadata)
		{
			_metadata = metadata
				.GroupBy(n => n.Key, n => n.Value)
				.ToDictionary(n => n.Key, n => n.ToList());
		}

		public IRequestMetadata GetMetadata(string requestName)
		{
			return new CompositeRequestMetadata(GetItems());

			IEnumerable<IRequestMetadata> GetItems()
			{
				if (_metadata.TryGetValue(requestName, out var metadata))
					foreach (var meta in metadata) yield return meta;

				if (_metadata.TryGetValue(DefaultMetadataKey, out var defaultMetadata))
					foreach (var meta in defaultMetadata) yield return meta;
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
			return Add(DefaultMetadataKey, defaultMetadata);
		}

		public IEnumerator<IRequestMetadata> GetEnumerator()
		{
			return _metadata.Values.SelectMany(n => n).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
