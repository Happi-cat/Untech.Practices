using System.Globalization;

namespace Untech.Practices.Localization.Sources
{
	public interface ILocalizationPartition
	{
		string Key { get; }

		CultureInfo Culture { get; }

		string GetString(string key);
	}
}