using System;
using System.Collections.Generic;
using System.Globalization;

namespace Untech.Practices.Localization.Sources
{
	public class LocalizationPartition : ILocalizationPartition
	{
		private readonly IReadOnlyDictionary<string, string> _translations;

		public LocalizationPartition(string name, CultureInfo culture,
			IReadOnlyDictionary<string, string> translations)
		{
			Name = name;
			Culture = culture;
			_translations = translations ?? throw new ArgumentNullException(nameof(translations));
		}

		public string Name { get; }
		public CultureInfo Culture { get; }

		public string GetString(string name)
		{
			return _translations.TryGetValue(name, out var translation) ? translation : null;
		}
	}
}