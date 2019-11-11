using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Untech.Practices.Localization
{
	public class LocalizationSourceProvider : ILocalizationSourceProvider
	{
		private readonly IReadOnlyDictionary<SourceKey, ILocalizationSource> _sources;

		public LocalizationSourceProvider(IEnumerable<ILocalizationSource> sources)
		{
			_sources = sources.ToDictionary(n => new SourceKey(n.Source, n.Culture), n => n);
		}

		public ILocalizationSource GetSource(string source, CultureInfo culture)
		{
			return _sources.TryGetValue(new SourceKey(source, culture), out var localizationSource)
				? localizationSource
				: null;
		}
	}
}