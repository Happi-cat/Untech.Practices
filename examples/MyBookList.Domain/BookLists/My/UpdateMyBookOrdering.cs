using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace MyBookList.Domain.BookLists.My
{
	[DataContract]
	public class UpdateMyBookOrdering : ICommand
	{
		public UpdateMyBookOrdering(int key, MyBookStatus status)
		{
			Key = key;
			Status = status;
		}

		[DataMember]
		public int Key { get; private set; }

		[DataMember]
		public MyBookStatus Status { get; private set; }
	}
}