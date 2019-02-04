using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace MyBookList.Domain.BookLists.My
{
	[DataContract]
	public class UpdateMyBookOrdering : ICommand
	{
		public UpdateMyBookOrdering(int key, int? ordering)
		{
			Key = key;
			Ordering = ordering;
		}

		[DataMember]
		public int Key { get; private set; }

		[DataMember]
		public int? Ordering { get; private set; }
	}
}