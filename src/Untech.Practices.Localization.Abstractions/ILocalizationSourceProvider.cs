using System.Globalization;

namespace Untech.Practices.Localization
{
	public interface ILocalizationSourceProvider
	{
		ILocalizationSource GetSource(string source, CultureInfo culture);
	}
}