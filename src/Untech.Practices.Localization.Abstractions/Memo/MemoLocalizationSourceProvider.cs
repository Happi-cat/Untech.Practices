using System;
using System.Collections.Generic;
using System.Globalization;

namespace Untech.Practices.Localization.Memo
{
	using SourceKey = Tuple<string, CultureInfo>;

	public class MemoLocalizationSourceProvider : ILocalizationSourceProvider
	{
		private readonly ILocalizationSourceProvider _sourceProvider;
		private readonly IDictionary<SourceKey, ILocalizationSource> _sources;

		public MemoLocalizationSourceProvider(ILocalizationSourceProvider sourceProvider)
		{
			_sourceProvider = sourceProvider ?? throw new ArgumentNullException(nameof(sourceProvider));
			_sources = new Dictionary<SourceKey, ILocalizationSource>();
		}

		public ILocalizationSource GetSource(string source, CultureInfo culture)
		{
			return _sources.TryGetValue(new SourceKey(source, culture), out ILocalizationSource localizationSource)
				? localizationSource
				: InitSource(source, culture);
		}

		private ILocalizationSource InitSource(string source, CultureInfo culture)
		{
			var localizationSource = _sourceProvider.GetSource(source, culture);

			_sources.Add(new SourceKey(source, culture), localizationSource);

			return localizationSource;
		}
	}
}