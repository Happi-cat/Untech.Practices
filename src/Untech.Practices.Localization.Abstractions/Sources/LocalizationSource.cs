using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Untech.Practices.Localization.Sources
{
	public class LocalizationSource : ILocalizationSource
	{
		private readonly IReadOnlyDictionary<PartitionKey, ILocalizationPartition> _partitions;

		public LocalizationSource(IEnumerable<ILocalizationPartition> partitions)
		{
			_partitions = partitions.ToDictionary(
				n => new PartitionKey(n.Name, n.Culture),
				n => n
			);
		}

		public ILocalizationPartition GetPartition(string name, CultureInfo culture)
		{
			return _partitions.TryGetValue(new PartitionKey(name, culture), out var partition)
				? partition
				: null;
		}
	}
}