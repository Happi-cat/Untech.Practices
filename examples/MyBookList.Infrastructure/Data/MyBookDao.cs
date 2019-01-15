using MyBookList.Domain.BookLists.My;
using Shared.Infrastructure.Data;

namespace MyBookList.Infrastructure.Data
{
	public class MyBookDao : IUserScopedDao
	{
		private MyBookDao()
		{
		}

		public MyBookDao(int key, int userKey, string author, string title)
		{
			Key = key;
			UserKey = userKey;
			Author = author;
			Title = title;
		}

		public int Key { get; private set; }
		public int UserKey { get; private set; }
		public int? BookKey { get; set; }
		public int? Ordering { get; set; }
		public string Author { get; private set; }
		public string Title { get; private set; }
		public string Review { get; set; }
		public MyBookStatus Status { get; set; }
	}
}