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

		public ILocalizationPartition GetPartition(string name)
		{
			var currentCulture = _culture ?? CultureInfo.InvariantCulture;
			do
			{
				var partition = _source.GetPartition(name, currentCulture);
				if (partition != null || currentCulture.Equals(currentCulture.Parent))
					return partition;
				currentCulture = currentCulture.Parent;
			} while (true);
		}
	}
}