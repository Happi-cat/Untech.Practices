using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Untech.Practices.Collections
{
	public class HierarchicalTest
	{
		private readonly Tree _tree = new Tree("earth")
		{
			new Tree("tree1")
			{
				new Tree("tree1.branch1") { new Tree("tree1.branch1.leaf") },
				new Tree("tree1.branch2")
				{
					new Tree("tree1.branch2.leaf1"), new Tree("tree1.branch2.leaf2")
				},
			},
			new Tree("tree2")
			{
				new Tree("tree2.branch")
				{
					new Tree("tree2.branch.leaf1"), new Tree("tree2.branch.leaf2")
				},
			}
		};


		[Fact]
		public void Elements_ReturnsElementsOnly()
		{
			var expected = new[] { "tree1", "tree2" };
			var actual = _tree.Elements().Select(n => n.ToString());
			Assert.Equal(expected, actual);
		}


		[Fact]
		public void Descendants_ReturnsAllDescendants()
		{
			var expected = new[]
			{
				"tree1",
				"tree1.branch1",
				"tree1.branch1.leaf",
				"tree1.branch2",
				"tree1.branch2.leaf1",
				"tree1.branch2.leaf2",
				"tree2",
				"tree2.branch",
				"tree2.branch.leaf1",
				"tree2.branch.leaf2",
			};
			var actual = _tree.Descendants().Select(n => n.ToString()).ToList();
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void DescendantsAndSelf_ReturnsAllDescendantsAndSelf()
		{
			var expected = new[]
			{
				"earth",
				"tree1",
				"tree1.branch1",
				"tree1.branch1.leaf",
				"tree1.branch2",
				"tree1.branch2.leaf1",
				"tree1.branch2.leaf2",
				"tree2",
				"tree2.branch",
				"tree2.branch.leaf1",
				"tree2.branch.leaf2",
			};
			var actual = _tree.DescendantsAndSelf().Select(n => n.ToString()).ToList();
			Assert.Equal(expected, actual);
		}

		private class Tree : IHierarchical<Tree>, IEnumerable<Tree>
		{
			private readonly string _content;
			private readonly IList<Tree> _items;

			public Tree(string content)
			{
				_content = content;
				_items = new List<Tree>();
			}

			public void Add(Tree item)
			{
				_items.Add(item);
			}

			public IEnumerable<Tree> Elements()
			{
				return _items;
			}

			public IEnumerator<Tree> GetEnumerator()
			{
				return _items.GetEnumerator();
			}

			public override string ToString()
			{
				return _content;
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}
	}
}