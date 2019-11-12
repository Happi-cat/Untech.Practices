using System;
using System.Collections.Generic;
using System.Globalization;

namespace Untech.Practices.Localization.Sources.Memo
{
	public class MemoLocalizationSource : ILocalizationSource
	{
		private readonly ILocalizationSource _source;
		private readonly IDictionary<PartitionKey, ILocalizationPartition> _partitions;

		public MemoLocalizationSource(ILocalizationSource source)
		{
			_source = source ?? throw new ArgumentNullException(nameof(source));
			_partitions = new Dictionary<PartitionKey, ILocalizationPartition>();
		}

		public ILocalizationPartition GetPartition(string name, CultureInfo culture)
		{
			return _partitions.TryGetValue(new PartitionKey(name, culture), out ILocalizationPartition partition)
				? partition
				: LoadAndRegisterPartition(name, culture);
		}

		private ILocalizationPartition LoadAndRegisterPartition(string name, CultureInfo culture)
		{
			var localizationSource = _source.GetPartition(name, culture);

			_partitions.Add(new PartitionKey(name, culture), localizationSource);

			return localizationSource;
		}
	}
}