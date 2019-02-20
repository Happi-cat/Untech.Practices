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
	public class MyBookDataStorage : UserScopedGenericDataStorage<MyBook, MyBookDao, Mappers>,
		IQueryHandler<MyBooksQuery, IEnumerable<MyBook>>
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