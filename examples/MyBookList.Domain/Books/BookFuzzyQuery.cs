using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace MyBookList.Domain.Books
{
	[DataContract]
	public class BookSearchQuery : IQuery<IEnumerable<Book>>
	{
		private IReadOnlyList<string> _tags;

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
		public string Author { get; set; }

		[DataMember]
		public string Title { get; set; }

		[DataMember]
		public IReadOnlyList<string> Tags
		{
			get => _tags ?? (_tags = new List<string>());
			set => _tags = value;
		}
	}
}