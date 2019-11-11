using System.Globalization;

namespace Untech.Practices.Localization
{
	public interface ILocalizationSource
	{
		string Source { get; }

		CultureInfo Culture { get; }

		string GetString(string reference);
	}
}