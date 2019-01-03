using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace MyBookList.Domain.BookLists.My
{
	[DataContract]
	public class UpdateMyBookReview : ICommand
	{
		public UpdateMyBookReview(int key, string review)
		{
			Key = key;
			Review = review;
		}

		[DataMember]
		public int Key { get; private set; }

		[DataMember]
		public string Review { get; private set; }
	}
}