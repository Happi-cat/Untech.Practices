using System.Globalization;

namespace Untech.Practices.Localization.Sources
{
	public interface ILocalizationPartition
	{
		string Name { get; }

		CultureInfo Culture { get; }

		string GetString(string name);
	}
}