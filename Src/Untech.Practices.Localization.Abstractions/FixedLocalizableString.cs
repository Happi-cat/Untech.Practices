namespace Untech.Practices.Localization
{
	public struct FixedLocalizableString : ILocalizableString
	{
		public FixedLocalizableString(string reference, string source)
		{
			Reference = reference;
			Source = source;
		}

		public string Reference { get; }
		public string Source { get; }

		public string Localize(ILocalizationContext context)
		{
			return Reference;
		}

		public string Localize(ILocalizationContext context, params object[] args)
		{
			return string.Format(Reference, args);
		}
	}
}