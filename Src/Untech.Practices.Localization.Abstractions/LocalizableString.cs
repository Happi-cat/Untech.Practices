namespace Untech.Practices.Localization
{
	public struct LocalizableString : ILocalizableString
	{
		public LocalizableString(string reference, string source)
		{
			Reference = reference;
			Source = source;
		}

		public string Reference { get; }
		public string Source { get; }

		public string Localize(ILocalizationContext context)
		{
			return context.GetSource(Source).GetString(Reference);
		}

		public string Localize(ILocalizationContext context, params object[] args)
		{
			var localizedString = Localize(context);
			return string.Format(localizedString, args);
		}
	}
}