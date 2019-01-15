using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MoreLinq;
using MyBookList.Domain.BookLists.My;
using MyBookList.Infrastructure.Data;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;

namespace MyBookList.Infrastructure
{
	public class BooksToReadQueryService : IQueryAsyncHandler<BooksToReadQuery, IEnumerable<MyBook>>
	{
		private readonly IQueryDispatcher _queryDispatcher;

		public BooksToReadQueryService(IQueryDispatcher queryDispatcher)
		{
			_queryDispatcher = queryDispatcher;
		}

		public async Task<IEnumerable<MyBook>> HandleAsync(BooksToReadQuery request,
			CancellationToken cancellationToken)
		{
			var pendingBooks = await FetchPendingBooksAsync(cancellationToken);

			pendingBooks = request.GetLucky
				? pendingBooks.Shuffle()
				: pendingBooks.OrderByDescending(b => b.Ordering);

			return pendingBooks.Take(10).ToList();
		}

		private Task<IEnumerable<MyBook>> FetchPendingBooksAsync(CancellationToken cancellationToken)
		{
			return _queryDispatcher.FetchAsync(new MyBooksQuery
			{
				OnlyStatuses = new[] { MyBookStatus.Pending, MyBookStatus.Postponed }
			}, cancellationToken);
		}
	}
}