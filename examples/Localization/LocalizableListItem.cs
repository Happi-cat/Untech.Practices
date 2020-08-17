using System.Collections.Generic;
using Untech.Practices.Localization;

namespace Localization
{
	public class LocalizableListItem
	{
		public LocalizableString Name { get; set; }
		public LocalizableString Description { get; set; }

		public static IEnumerable<LocalizableListItem> Create()
		{
			yield return new LocalizableListItem
			{
				Name = new LocalizableString("home", "web"),
				Description = new LocalizableString("home:description", "web")
			};
			yield return new LocalizableListItem
			{
				Name = new LocalizableString("contacts", "web"),
				Description = new LocalizableString("contacts:description", "web")
			};
			yield return new LocalizableListItem
			{
				Name = new LocalizableString("about", "web"),
				Description = new LocalizableString("about:description", "web")
			};
		}
	}
}