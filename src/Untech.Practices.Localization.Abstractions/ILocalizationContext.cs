using Untech.Practices.Localization.Sources;

namespace Untech.Practices.Localization
{
	public interface ILocalizationContext
	{
		ILocalizationPartition GetPartition(string name);
	}
}