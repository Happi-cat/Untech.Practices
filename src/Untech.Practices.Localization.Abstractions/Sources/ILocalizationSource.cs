using System.Globalization;

namespace Untech.Practices.Localization.Sources
{
	public interface ILocalizationSource
	{
		ILocalizationPartition GetPartition(string key, CultureInfo culture);
	}
}