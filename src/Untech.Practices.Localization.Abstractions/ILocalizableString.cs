namespace Untech.Practices.Localization
{
	public interface ILocalizableString
	{
		string Reference { get; }

		string Source { get; }

		string Localize(ILocalizationContext context);

		string Localize(ILocalizationContext context, params object[] args);
	}
}