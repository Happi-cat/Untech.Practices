using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Runtime.Serialization;
using MyBookList.Domain.Library;
using Untech.Practices.DataStorage;

namespace MyBookList.Domain.BookLists.Shared
{
	[DataContract]
	public class SharedBookList : IHasKey
	{
		private SharedBookList()
		{

		}

		public SharedBookList(int key, string title, IEnumerable<string> tags, IEnumerable<Book> books, string description = null)
		{
			Key = key;
			Title = title;
			Description = description;
			Tags = tags?.ToList() ?? new List<string>();
			Books = books?.ToList() ?? new List<Book>();
		}

		[DataMember]
		public int Key { get; private set; }

		[DataMember]
		public string Title { get; private set; }

		[DataMember]
		public string Description { get; private set; }

		[DataMember]
		public IReadOnlyList<string> Tags { get; private set; }

		[DataMember]
		public IReadOnlyList<Book> Books { get; private set; }
	}
}