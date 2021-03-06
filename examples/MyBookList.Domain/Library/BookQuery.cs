using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace MyBookList.Domain.Library
{
	[DataContract]
	public class BookQuery : IQuery<Book>
	{
		public BookQuery(int key)
		{
			Key = key;
		}

		[DataMember]
		public int Key { get; private set; }
	}
}