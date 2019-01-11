using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;
using MyBookList.Domain.BookLists.My;
using Shared.Infrastructure.Data;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.UserContext;

namespace MyBookList.Infrastructure.Data
{
	public class MyBookDao : IUserScopedDao
	{
		public int Key { get; set; }
		public int UserKey { get; set; }
		public int? BookKey { get; set; }
		public int? Ordering { get; set; }
		public string Author { get; set; }
		public string Title { get;  set; }
		public string Review { get;  set; }
		public MyBookStatus Status { get; set; }
	}

	public struct Mappers : IDaoMapper<MyBook, MyBookDao>
	{
		public MyBook FromDao(IUserContext userContext, MyBookDao dao)
		{
			throw new System.NotImplementedException();
		}

		public MyBookDao ToDao(IUserContext userContext, MyBook entity)
		{
			throw new System.NotImplementedException();
		}
	}

	public class MyBookDataStorage : UserScopedGenericDataStorage<MyBook, MyBookDao, Mappers>,
		IQueryAsyncHandler<MyBooksQuery, IEnumerable<MyBook>>
	{
		public MyBookDataStorage(IUserContext userContext, Func<IDataContext> dataContext)
			: base(userContext, dataContext)
		{
		}

		public async Task<IEnumerable<MyBook>> HandleAsync(MyBooksQuery request, CancellationToken cancellationToken)
		{
			using (var context = GetContext())
			{
				var query = GetMyItems(context);

				if (request.OnlyStatuses.Count > 0)
				{
					query = query.Where(b => request.OnlyStatuses.Contains(b.Status));
				}

				var daos = await query.ToListAsync(cancellationToken);

				return daos.Select(FromDao);
			}
		}
	}
}