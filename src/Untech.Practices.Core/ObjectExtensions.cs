namespace Untech.Practices.Core
{
	public static class ObjectExtensions
	{
		public static string NullIfEmpty(this string arg)
		{
			return string.IsNullOrEmpty(arg) ? null : arg;
		}

		public static string NullIfWhiteSpace(this string arg)
		{
			return string.IsNullOrWhiteSpace(arg) ? null : arg;
		}
	}
}