using System;

namespace Untech.Practices.Localization
{
	public static class LocalizationContextExtensions
	{
		public static string Localize(this ILocalizationContext context, string partition, string name)
		{
			if (context == null) throw new ArgumentNullException(nameof(context));

			return context.GetPartition(partition).GetString(name);
		}

		public static string Localize(this ILocalizationContext context, string partition, string name, params object[] args)
		{
			return string.Format(context.Localize(partition, name), args);
		}
	}
}