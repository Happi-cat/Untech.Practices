using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace MyBookList.Domain.BookLists.My
{
	[DataContract]
	public class AppendExistingBookToMyBookList : ICommand
	{
		public AppendExistingBookToMyBookList(int bookKey)
		{
			BookKey = bookKey;
		}

		[DataMember]
		public int BookKey { get; private set; }
	}
}