using System.Collections.Generic;
using System.Globalization;

namespace Untech.Practices.Localization
{
	public class LocalizationContext : ILocalizationContext
	{
		private readonly CultureInfo _culture;
		private readonly ILocalizationSourceProvider _sourceProvider;

		public LocalizationContext(CultureInfo culture, ILocalizationSourceProvider sourceProvider)
		{
			_culture = culture;
			_sourceProvider = sourceProvider;
		}

		public ILocalizationSource GetSource(string source)
		{
			var currentCulture = _culture ?? CultureInfo.InvariantCulture;
			do
			{
				var localizationSource = _sourceProvider.GetSource(source, currentCulture);
				if (localizationSource != null || currentCulture.Equals(currentCulture.Parent)) return localizationSource;
				currentCulture = currentCulture.Parent;
			} while (true);
		}
	}
}