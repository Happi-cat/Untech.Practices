using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Untech.Practices.Localization.Sources
{
	public class LocalizationSource : ILocalizationSource
	{
		private readonly IReadOnlyDictionary<PartitionKey, ILocalizationPartition> _sources;

		public LocalizationSource(IEnumerable<ILocalizationPartition> sources)
		{
			_sources = sources.ToDictionary(
				n => new PartitionKey(n.Key, n.Culture),
				n => n
			);
		}

		public ILocalizationPartition GetPartition(string key, CultureInfo culture)
		{
			return _sources.TryGetValue(new PartitionKey(key, culture), out var localizationSource)
				? localizationSource
				: null;
		}
	}
}