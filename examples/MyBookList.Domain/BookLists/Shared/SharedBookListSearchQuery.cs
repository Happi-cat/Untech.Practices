using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace MyBookList.Domain.BookLists.Shared
{
	[DataContract]
	public class SharedBookListSearchQuery : IQuery<IEnumerable<SharedBookList>>
	{
		private IReadOnlyList<string> _tags;

		public SharedBookListSearchQuery(int count)
		{
			Count = count;
		}

		[DataMember]
		public int Count { get; private set; }

		[DataMember]
		public IReadOnlyList<string> Tags
		{
			get => _tags ?? (_tags = new List<string>());
			set => _tags = value;
		}
	}
}