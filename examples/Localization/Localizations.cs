using System.Collections.Generic;
using System.Globalization;
using Untech.Practices.Localization.Sources;

namespace Localization
{
	public static class Localizations
	{
		public static ILocalizationSource Load()
		{
			return new LocalizationSource(new[]
			{
				new LocalizationPartition("web", CultureInfo.InvariantCulture,
					new Dictionary<string, string>
					{
						{ "home", "Home" },
						{ "home:description", "Go to home page" },
						{ "contacts", "Contacts" },
						{ "contacts:description", "See out contacts" },
						{ "about", "About Us" },
						{ "about:description", "Read more info about us" },
					}),
				new LocalizationPartition("web", new CultureInfo("ru-RU"),
					new Dictionary<string, string>
					{
						{ "home:description", "Перейти на домашнюю страницу" },
						{ "contacts:description", "Контактная информация" },
						{ "about:description", "Кто мы" },
					})
			});
		}
	}
}