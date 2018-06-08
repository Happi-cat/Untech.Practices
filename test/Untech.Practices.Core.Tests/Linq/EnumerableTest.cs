using System;
using System.Linq;
using Untech.Practices.Linq;
using Xunit;

namespace Untech.Practices.Collections.Linq
{
	public class EnumerableTest
	{
		[Fact]
		public void ToChunks_ThrowsArgNull_WhenSourceIsNull()
		{
			Assert.Throws<ArgumentNullException>(() => EnumerableExtensions.ToChunks<int>(null, 10).ToList());
		}

		[Fact]
		public void ToChunks_ThrowsOutOfRange_WhenChunkSizeIsLE0()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => new [] { 1 }.ToChunks(0).ToList());
			Assert.Throws<ArgumentOutOfRangeException>(() => new [] { 1 }.ToChunks(-1).ToList());
		}

		[Fact]
		public void ToChunks_ReturnsChunk_WhenSourceSmall()
		{
			var original = new[] { 1 };
			var expected = new[]
			{
				new[] { 1 }
			};

			Assert.Equal(expected, original.ToChunks(4));
		}

		[Fact]
		public void ToChunks_ReturnsChunks_WhenSourceLarge()
		{
			var original = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			var expected = new[]
			{
				new[] { 1, 2, 3, 4 },
				new[] { 5, 6, 7, 8 },
				new[] { 9 }
			};

			Assert.Equal(expected, original.ToChunks(4));
		}
	}
}