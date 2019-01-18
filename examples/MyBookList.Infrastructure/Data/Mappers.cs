using MyBookList.Domain.BookLists.My;
using Shared.Infrastructure.Data;
using Untech.Practices.UserContext;

namespace MyBookList.Infrastructure.Data
{
	public struct Mappers : IDaoMapper<MyBook, MyBookDao>
	{
		public MyBook FromDao(IUserContext userContext, MyBookDao dao)
		{
			return new MyBook(dao.Key, dao.BookKey, dao.Author, dao.Title, dao.Status, dao.Ordering, dao.Review);
		}

		public MyBookDao ToDao(IUserContext userContext, MyBook entity)
		{
			return new MyBookDao(entity.Key, userContext.UserKey, entity.Author, entity.Title)
			{
				BookKey = entity.BookKey,
				Ordering = entity.Ordering,
				Review = entity.Review,
				Status = entity.Status
			};
		}
	}
}