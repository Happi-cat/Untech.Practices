using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace MyBookList.Domain.BookLists.My
{
	[DataContract]
	public class BooksToReadQuery : IQuery<IEnumerable<MyBook>>
	{
		[DataMember]
		public bool GetLucky { get; set; }
	}
}