using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace MyBookList.Domain.BookLists.My
{
	[DataContract]
	public class AppendNewBookToMyBookList : ICommand
	{
		public AppendNewBookToMyBookList(string title, string author, IEnumerable<string> tags)
		{
			Title = title;
			Author = author;
			Tags = tags?.ToList() ?? new List<string>();
		}

		[DataMember]
		public string Title { get; private set; }

		[DataMember]
		public string Author { get; private set; }

		[DataMember]
		public IReadOnlyList<string> Tags { get; private set; }
	}
}