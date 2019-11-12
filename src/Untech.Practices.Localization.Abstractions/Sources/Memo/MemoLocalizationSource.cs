using System;
using System.Collections.Generic;
using System.Globalization;

namespace Untech.Practices.Localization.Sources.Memo
{
	public class MemoLocalizationSource : ILocalizationSource
	{
		private readonly ILocalizationSource _source;
		private readonly IDictionary<PartitionKey, ILocalizationPartition> _sources;

		public MemoLocalizationSource(ILocalizationSource source)
		{
			_source = source ?? throw new ArgumentNullException(nameof(source));
			_sources = new Dictionary<PartitionKey, ILocalizationPartition>();
		}

		public ILocalizationPartition GetPartition(string key, CultureInfo culture)
		{
			return _sources.TryGetValue(new PartitionKey(key, culture), out ILocalizationPartition localizationSource)
				? localizationSource
				: InitPartition(key, culture);
		}

		private ILocalizationPartition InitPartition(string key, CultureInfo culture)
		{
			var localizationSource = _source.GetPartition(key, culture);

			_sources.Add(new PartitionKey(key, culture), localizationSource);

			return localizationSource;
		}
	}
}