using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace MyBookList.Domain.Books
{
	[DataContract]
	public class BookSearchQuery : IQuery<IEnumerable<Book>>
	{
		private BookSearchQuery()
		{

		}

		public BookSearchQuery(int count,
			string author = null,
			string title = null,
			IEnumerable<string> tags = null)
		{
			Count = count;
		}

		[DataMember]
		public int Count { get; private set; }

		[DataMember]
		public string Author { get; private set; }

		[DataMember]
		public string Title { get; private set; }

		[DataMember]
		public IReadOnlyList<string> Tags { get; private set; }
	}
}