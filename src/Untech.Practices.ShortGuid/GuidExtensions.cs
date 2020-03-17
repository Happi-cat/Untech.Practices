using System;
using System.Buffers.Text;
using System.Runtime.InteropServices;

namespace Untech.Practices.ShortGuid
{
	public static class GuidExtensions
	{
		private const byte ForwardSlashByte = (byte)'/';
		private const char Underscore = '_';

		private const byte PlusByte = (byte)'+';
		private const char Dash = '-';

		private const byte Equal = (byte)'=';

		public static string ToShortGuid(this Guid guid)
		{
			Span<byte> guidBytes = stackalloc byte[16];
			Span<byte> encodedBytes = stackalloc byte[24];

			MemoryMarshal.TryWrite(guidBytes, ref guid); // write bytes from the Guid
			Base64.EncodeToUtf8(guidBytes, encodedBytes, out _, out _);

			Span<char> chars = stackalloc char[22];

			// replace any characters which are not URL safe
			// skip the final two bytes as these will be '==' padding we don't need
			for (var i = 0; i < 22; i++)
			{
				switch (encodedBytes[i])
				{
					case ForwardSlashByte:
						chars[i] = Underscore;
						break;
					case PlusByte:
						chars[i] = Dash;
						break;
					default:
						chars[i] = (char)encodedBytes[i];
						break;
				}
			}

			return new string(chars);
		}

		public static Guid ParseShortGuid(this string str)
		{
			if (str == null || str.Length != 22) return default(Guid);

			ReadOnlySpan<char> chars = str.AsSpan();

			Span<byte> encodedBytes = stackalloc byte[24];
			encodedBytes[23] = Equal;
			encodedBytes[22] = Equal;

			for (var i = 0; i < 22; i++)
			{
				switch (chars[i])
				{
					case Dash:
						encodedBytes[i] = PlusByte;
						break;
					case Underscore:
						encodedBytes[i] = ForwardSlashByte;
						break;
					default:
						encodedBytes[i] = (byte) chars[i];
						break;
				}
			}

			Span<byte> guidBytes = stackalloc byte[16];

			Base64.DecodeFromUtf8(encodedBytes, guidBytes, out _, out _);

			return new Guid(guidBytes);
		}
	}
}