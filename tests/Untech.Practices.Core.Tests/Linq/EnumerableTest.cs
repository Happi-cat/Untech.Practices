using System;
using System.Collections.Generic;
using System.Linq;
using Untech.Practices.Linq;
using Xunit;

namespace Untech.Practices.Collections.Linq
{
	public class EnumerableTest
	{
		[Fact]
		public void OrderByPosition_ReturnsAllOrdered_WhenFewKeysPredefinedAndComparer()
		{
			int[] original = { 6, 5, 4, 3, 2, 1 };
			int[] predefined = { 2, 4, 6 };
			int[] expected = { 2, 4, 6, 1, 3, 5 };

			List<int> ordered = original
				.OrderByPosition(n => n, predefined, Comparer<int>.Default)
				.ToList();

			Assert.Equal(expected, ordered);
			Assert.NotEqual(original, ordered);
		}

		[Fact]
		public void OrderByPosition_ReturnsOrdered_WhenAllKeysPredefined()
		{
			int[] original = { 1, 2, 3, 4, 5, 6 };
			int[] expected = { 3, 2, 1, 6, 5, 4 };

			List<int> ordered = original.OrderByPosition(n => n, expected).ToList();

			Assert.Equal(expected, ordered);
			Assert.NotEqual(original, ordered);
		}

		[Fact]
		public void OrderByPosition_ReturnsOrderedPredefinedAndUnorderedUndefinedKeys_WhenFewKeysPredefined()
		{
			int[] original = { 6, 1, 4, 5, 2, 3 };
			int[] predefined = { 2, 4, 6 };
			int[] expected = { 2, 4, 6, 1, 5, 3 };

			List<int> ordered = original.OrderByPosition(n => n, predefined).ToList();

			Assert.Equal(expected, ordered);
			Assert.NotEqual(original, ordered);
		}

		[Fact]
		public void ToChunks_ReturnsChunk_WhenSourceSmall()
		{
			int[] original = { 1 };
			int[][] expected =
			{
				new[] { 1 }
			};

			Assert.Equal(expected, original.ToChunks(4));
		}

		[Fact]
		public void ToChunks_ReturnsChunks_WhenSourceLarge()
		{
			int[] original = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			int[][] expected =
			{
				new[] { 1, 2, 3, 4 },
				new[] { 5, 6, 7, 8 },
				new[] { 9 }
			};

			Assert.Equal(expected, original.ToChunks(4));
		}

		[Fact]
		public void ToChunks_ThrowsArgNull_WhenSourceIsNull()
		{
			Assert.Throws<ArgumentNullException>(() => EnumerableExtensions.ToChunks<int>(null, 10));
		}

		[Fact]
		public void ToChunks_ThrowsOutOfRange_WhenChunkSizeIsLE0()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => new[] { 1 }.ToChunks(0));
			Assert.Throws<ArgumentOutOfRangeException>(() => new[] { 1 }.ToChunks(-1));
		}
	}
}