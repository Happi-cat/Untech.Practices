using System.Globalization;
using System.Linq;
using Untech.Practices.Localization;

namespace Localization
{
	public static class Demo
	{
		public static void Main()
		{
			var sourceProvider = Localizations.Load();
			var model = ListItem.Create();

			var languages = new[] { "en", "en-gb", "ru", "ru-ru" };

			var localizedModels = languages
				.Select(l => new CultureInfo(l))
				.Select(c => new LocalizationContext(c, sourceProvider))
				.Select(ctx => model.Localize(ctx))
				.ToList();
		}
	}
}