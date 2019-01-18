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
		public IReadOnlyList<MyBook> Books { get; private set; }
	}
}