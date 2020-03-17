using System;
using Xunit;

namespace Untech.Practices.ShortGuid
{
	public class GuidExtensionsTests
	{
		[Fact]
		public void ToShortGuid()
		{
			var guid = Guid.NewGuid();
			var expected = ToBase64String(guid);

			var actual = guid.ToShortGuid();

			Assert.Equal(expected, actual);
		}

		[Fact]
		public void ParseShortGuid()
		{
			var expected = Guid.NewGuid();
			var base64Guid = ToBase64String(expected);

			var actual = base64Guid.ParseShortGuid();

			Assert.Equal(expected, actual);
		}

		[Fact]
		public void ParseShortGuid_ReturnsDefault_IfWrongLength()
		{
			Assert.Equal(Guid.Empty, "".ParseShortGuid());
			Assert.Equal(Guid.Empty, "abc".ParseShortGuid());
			Assert.Equal(Guid.Empty, "abc123.abc123.abc123.abc123.abc123.abc123".ParseShortGuid());
		}

		[Fact]
		public void ParseShortGuid_ReturnsDefault_IfNull()
		{
			Assert.Equal(Guid.Empty, ((string)null).ParseShortGuid());
		}

		private static string ToBase64String(Guid guid)
		{
			string encoded = Convert.ToBase64String(guid.ToByteArray());
			encoded = encoded.Replace("/", "_").Replace("+", "-");
			return encoded.Substring(0, 22);
		}
	}
}