using System.Linq;
using Xunit;

namespace Untech.Practices.Collections
{
	public class RoundRobinCollectionTest
	{
		[Fact]
		public void Run()
		{
			var collection = new RoundRobinCollection<int>();

			foreach (var val in Enumerable.Range(0, 20))
			{
				collection.Push(val);
			}

			var arr1 = collection.Take(20).ToList();
			var arr1_1 = collection.Take(10).ToList();
			var arr1_2 = collection.Take(5).ToList();
			var arr1_3 = collection.Take(5).ToList();

			Assert.Equal(0, collection.Peek());
			Assert.Equal(1, collection.Peek());
			Assert.Equal(2, collection.Pop());
			Assert.Equal(3, collection.Peek());
			Assert.Equal(4, collection.First());
			Assert.Equal(5, collection.Take(5).First());
		}
	}
}
