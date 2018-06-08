using System;
using System.Collections.Generic;
using System.Linq;
using Untech.Practices.Linq;
using Xunit;

namespace Untech.Practices.Collections.Linq
{
	public class EnumerableTest
	{
		#region [OrderByPosition]

		[Fact]
		public void OrderByPosition_ReturnsOrdered_WhenAllKeysPredefined()
		{
			var original = new[] { 1, 2, 3, 4, 5, 6 };
			var expected = new[] { 3, 2, 1, 6, 5, 4 };

			var ordered = original.OrderByPosition(n => n, expected).ToList();

			Assert.Equal(expected, ordered);
			Assert.NotEqual(original, ordered);
		}

		[Fact]
		public void OrderByPosition_ReturnsOrderedPredefinedAndUnorderedUndefinedKeys_WhenFewKeysPredefined()
		{
			var original = new[] { 6, 1, 4, 5, 2, 3 };
			var predefined = new[] { 2, 4, 6 };
			var expected = new[] { 2, 4, 6, 1, 5, 3 };

			var ordered = original.OrderByPosition(n => n, predefined).ToList();

			Assert.Equal(expected, ordered);
			Assert.NotEqual(original, ordered);
		}

		[Fact]
		public void OrderByPosition_ReturnsAllOrdered_WhenFewKeysPredefinedAndComparer()
		{
			var original = new[] { 6, 5, 4, 3, 2, 1 };
			var predefined = new[] { 2, 4, 6 };
			var expected = new[] { 2, 4, 6, 1, 3, 5 };

			var ordered = original
				.OrderByPosition(n => n, predefined, Comparer<int>.Default)
				.ToList();

			Assert.Equal(expected, ordered);
			Assert.NotEqual(original, ordered);
		}

		#endregion

		#region [ToChunks]

		[Fact]
		public void ToChunks_ThrowsArgNull_WhenSourceIsNull()
		{
			Assert.Throws<ArgumentNullException>(() => EnumerableExtensions.ToChunks<int>(null, 10));
		}

		[Fact]
		public void ToChunks_ThrowsOutOfRange_WhenChunkSizeIsLE0()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => new [] { 1 }.ToChunks(0));
			Assert.Throws<ArgumentOutOfRangeException>(() => new [] { 1 }.ToChunks(-1));
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

		#endregion
	}
}