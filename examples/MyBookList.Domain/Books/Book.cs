using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.Practices.DataStorage;

namespace MyBookList.Domain.Books
{
	[DataContract]
	public class Book : IAggregateRoot
	{
		[DataMember]
		public int Key { get; private set; }

		[DataMember]
		public string Author { get; private set; }

		[DataMember]
		public string Title { get; private set; }

		[DataMember]
		public string Description { get; private set; }

		[DataMember]
		public IReadOnlyList<string> Tags { get; private set; }
	}
}