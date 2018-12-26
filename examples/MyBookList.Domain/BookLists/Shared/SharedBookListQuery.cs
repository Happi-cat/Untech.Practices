using Untech.Practices.CQRS;

namespace MyBookList.Domain.BookLists.Shared
{
	public class SharedBookListQuery : IQuery<SharedBookList>
	{
		public SharedBookListQuery(int key)
		{
			Key = key;
		}

		public int Key { get; private set; }
	}
}