using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Untech.Practices.DataStorage;

namespace MyBookList.Domain.Library
{
	[DataContract]
	public class Book : IHasKey
	{
		private Book()
		{

		}

		public Book(int key, string author, string title, IEnumerable<string> tags, string description = null)
		{
			Key = key;
			Author = author;
			Title = title;
			Tags = tags?.ToList() ?? new List<string>();
			Description = description;
		}

		[DataMember]
		public int Key { get; private set; }

		[DataMember]
		public string Author { get; private set; }

		[DataMember]
		public string Title { get; private set; }

		[DataMember]
		public string Description { get; private set; }

		[DataMember]
		public IReadOnlyList<string> Tags { get; private set; }
	}
}