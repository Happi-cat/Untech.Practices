using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace MyBookList.Domain.Library
{
	[DataContract]
	public class BookSearchQuery : IQuery<IEnumerable<Book>>
	{
		private IReadOnlyList<string> _tags;

		public BookSearchQuery(int count)
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