using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MyBookList.Domain.BookLists.My
{
	[DataContract]
	public class MyBookList
	{
		private MyBookList()
		{

		}

		public MyBookList(IEnumerable<MyBook> books)
		{
			Books = books?.ToList() ?? new List<MyBook>();

		}

		[DataMember]
		public int PendingBooksCount => Books.Count(n => n.Status == MyBookStatus.Pending);

		[DataMember]
		public int ReadingBooksCount => Books.Count(n => n.Status == MyBookStatus.Reading);

		[DataMember]
		public int CompletedBooksCount => Books.Count(n => n.Status == MyBookStatus.Completed);

		[DataMember]
		public int TotalBooksCount => Books.Count;

		[DataMember]
		public IReadOnlyList<MyBook> Books { get; private set; }
	}
}