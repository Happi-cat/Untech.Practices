using System.Globalization;
using System.Linq;
using Untech.Practices.Localization;

namespace Localization
{
	public static class Demo
	{
		public static void Main()
		{
			var localizationSource = Localizations.Load();
			var model = LocalizableListItem.Create();

			var languages = new[] { "en", "en-gb", "ru", "ru-ru" };

			var localizedModels = languages
				.Select(l => new CultureInfo(l))
				.Select(c => new LocalizationContext(c, localizationSource))
				.Select(ctx => model.Localize(ctx))
				.ToList();
		}
	}
}