using System;
using System.Collections.Generic;
using System.Globalization;

namespace Untech.Practices.Localization
{
	public class LocalizationSource : ILocalizationSource
	{
		private readonly IReadOnlyDictionary<string, string> _translations;

		public LocalizationSource(string source, CultureInfo culture,
			IReadOnlyDictionary<string, string> translations)
		{
			Source = source;
			Culture = culture;
			_translations = translations ?? throw new ArgumentNullException(nameof(translations));
		}

		public string Source { get; }
		public CultureInfo Culture { get; }

		public string GetString(string reference)
		{
			return _translations.TryGetValue(reference, out var translation) ? translation : null;
		}
	}
}