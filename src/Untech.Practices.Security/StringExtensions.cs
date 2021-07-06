using System;
using System.Buffers;

namespace Untech.Practices.Security
{
	internal static class StringExtensions
	{
		public static string ToUrlSafeString(this string str)
		{
			var src = str.AsSpan();

			var buffer = ArrayPool<char>.Shared.Rent(src.Length);
			var length = src.Length;

			// trim trailing = in the end
			while (src[length - 1] == '=') length--;

			// replace any characters which are not URL safe
			for (var i = 0; i < length; i++)
			{
				buffer[i] = src[i] switch
				{
					'+' => '-',
					'/' => '_',
					_ => src[i]
				};
			}

			var result = new string(buffer, 0, length);

			ArrayPool<char>.Shared.Return(buffer);

			return result;
		}
	}
}