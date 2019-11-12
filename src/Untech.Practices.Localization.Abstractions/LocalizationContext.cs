using System.Globalization;
using Untech.Practices.Localization.Sources;

namespace Untech.Practices.Localization
{
	public class LocalizationContext : ILocalizationContext
	{
		private readonly CultureInfo _culture;
		private readonly ILocalizationSource _source;

		public LocalizationContext(CultureInfo culture, ILocalizationSource source)
		{
			_culture = culture;
			_source = source;
		}

		public ILocalizationPartition GetPartition(string key)
		{
			var currentCulture = _culture ?? CultureInfo.InvariantCulture;
			do
			{
				var localizationSource = _source.GetPartition(key, currentCulture);
				if (localizationSource != null || currentCulture.Equals(currentCulture.Parent))
					return localizationSource;
				currentCulture = currentCulture.Parent;
			} while (true);
		}
	}
}