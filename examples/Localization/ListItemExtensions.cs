using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Untech.Practices.Localization;

namespace Localization
{
	public static class ListItemExtensions
	{
		public static ReadOnlyCollection<LocalizedListItem> Localize(this IEnumerable<ListItem> items,
			ILocalizationContext context)
		{
			return items.Select(i => Localize(i, context)).ToList().AsReadOnly();
		}

		public static LocalizedListItem Localize(this ListItem item, ILocalizationContext context)
		{
			return new LocalizedListItem
			{
				Name = L(item.Name),
				Description = L(item.Description)
			};

			string L(LocalizableString s) => s.Localize(context);
		}
	}
}