namespace Adr
{
	public static class StringExtensions
	{
		public static string TrimNewLines(this string str)
		{
			return str?.Trim('\n', '\r');
		}
	}
}