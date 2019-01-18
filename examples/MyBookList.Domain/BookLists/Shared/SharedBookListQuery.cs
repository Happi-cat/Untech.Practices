using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace MyBookList.Domain.BookLists.Shared
{
	[DataContract]
	public class SharedBookListQuery : IQuery<SharedBookList>
	{
		public SharedBookListQuery(int key)
		{
			Key = key;
		}

		[DataMember]
		public int Key { get; private set; }
	}
}