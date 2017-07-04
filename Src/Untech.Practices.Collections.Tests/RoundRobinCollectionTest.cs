using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Untech.Practices.Collections
{
	[TestClass]
	public class RoundRobinCollectionTest
	{
		[TestMethod]
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

			Assert.AreEqual(0, collection.Peek());
			Assert.AreEqual(1, collection.Peek());
			Assert.AreEqual(2, collection.Pop());
			Assert.AreEqual(3, collection.Peek());
			Assert.AreEqual(4, collection.First());
			Assert.AreEqual(5, collection.Take(5).First());
		}

	}
}
