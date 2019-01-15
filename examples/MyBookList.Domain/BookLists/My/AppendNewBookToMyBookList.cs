using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace MyBookList.Domain.BookLists.My
{
	[DataContract]
	public class AppendNewBookToMyBookList : ICommand<MyBook>
	{
		private AppendNewBookToMyBookList()
		{

		}

		public AppendNewBookToMyBookList(string title, string author)
		{
			Title = title;
			Author = author;
		}

		[DataMember]
		public string Title { get; private set; }

		[DataMember]
		public string Author { get; private set; }
	}
}