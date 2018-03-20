namespace Untech.Practices.Localization
{
	public interface ILocalizationContext
	{
		ILocalizationSource GetSource(string source);
	}
}