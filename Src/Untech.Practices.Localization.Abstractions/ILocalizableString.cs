namespace Untech.Practices.Localization
{
	public interface ILocalizableString
	{
		string Localize(ILocalizationContext context);

		string Localize(ILocalizationContext context, params object[] args);
	}
}