using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace MyBookList.Domain.BookLists.My
{
	[DataContract]
	public class AppendSharedBookListToMyBookList : ICommand<IEnumerable<MyBook>>
	{
		public AppendSharedBookListToMyBookList(int sharedBookListKey)
		{
			SharedBookListKey = sharedBookListKey;
		}

		[DataMember]
		public int SharedBookListKey { get; private set; }
	}
}