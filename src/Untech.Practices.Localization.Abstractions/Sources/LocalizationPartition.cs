using System;
using System.Collections.Generic;
using System.Globalization;

namespace Untech.Practices.Localization.Sources
{
	public class LocalizationPartition : ILocalizationPartition
	{
		private readonly IReadOnlyDictionary<string, string> _translations;

		public LocalizationPartition(string key, CultureInfo culture,
			IReadOnlyDictionary<string, string> translations)
		{
			Key = key;
			Culture = culture;
			_translations = translations ?? throw new ArgumentNullException(nameof(translations));
		}

		public string Key { get; }
		public CultureInfo Culture { get; }

		public string GetString(string key)
		{
			return _translations.TryGetValue(key, out var translation) ? translation : null;
		}
	}
}