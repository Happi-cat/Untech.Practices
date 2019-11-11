using System.Collections.Generic;
using System.Globalization;
using Untech.Practices.Localization;

namespace Localization
{
	public class ListItem
	{
		public LocalizableString Name { get; set; }
		public LocalizableString Description { get; set; }

		public static IEnumerable<ListItem> Create()
		{
			yield return new ListItem
			{
				Name = new LocalizableString("home", "web"),
				Description = new LocalizableString("home:description", "web")
			};
			yield return new ListItem
			{
				Name = new LocalizableString("contacts", "web"),
				Description = new LocalizableString("contacts:description", "web")
			};
			yield return new ListItem
			{
				Name = new LocalizableString("about", "web"),
				Description = new LocalizableString("about:description", "web")
			};
		}
	}

	public static class Localizations
	{
		public static ILocalizationSourceProvider Load()
		{
			return new LocalizationSourceProvider(new[]
			{
				new LocalizationSource("web", CultureInfo.InvariantCulture,
					new Dictionary<string, string>
					{
						{ "home", "Home" },
						{ "home:description", "Go to home page" },
						{ "contacts", "Contacts" },
						{ "contacts:description", "See out contacts" },
						{ "about", "About Us" },
						{ "about:description", "Read more info about us" },
					}),
				new LocalizationSource("web", new CultureInfo("ru-RU"),
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