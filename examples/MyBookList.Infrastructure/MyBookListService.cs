using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyBookList.Domain.BookLists.My;
using MyBookList.Domain.BookLists.Shared;
using MyBookList.Domain.Library;
using MyBookList.Infrastructure.Data;
using Untech.Practices;
using Untech.Practices.CQRS;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.DataStorage;
using MyList = MyBookList.Domain.BookLists.My.MyBookList;

namespace MyBookList.Infrastructure
{
	public class MyBookListService : IQueryHandler<MyBookListQuery, MyList>,
		ICommandHandler<AppendExistingBookToMyBookList, MyBook>,
		ICommandHandler<AppendNewBookToMyBookList, MyBook>,
		ICommandHandler<AppendSharedBookListToMyBookList, IEnumerable<MyBook>>,
		ICommandHandler<UpdateMyBookReview, None>,
		ICommandHandler<UpdateMyBookStatus, None>,
		ICommandHandler<UpdateMyBookOrdering, None>
	{
		private readonly IDataStorage<MyBook> _myBookDataStorage;
		private readonly IQueryDispatcher _queryDispatcher;

		public MyBookListService(IDataStorage<MyBook> myBookDataStorage, IQueryDispatcher queryDispatcher)
		{
			_myBookDataStorage = myBookDataStorage;
			_queryDispatcher = queryDispatcher;
		}

		public async Task<MyList> HandleAsync(MyBookListQuery request,
			CancellationToken cancellationToken)
		{
			var books = await _queryDispatcher.FetchAsync(new MyBooksQuery(), cancellationToken);

			return new MyList(books);
		}

		public async Task<MyBook> HandleAsync(AppendExistingBookToMyBookList request,
			CancellationToken cancellationToken)
		{
			var existingBook = await _queryDispatcher.FetchAsync(new BookQuery(request.BookKey), cancellationToken);

			var book = new MyBook(existingBook);

			book = await _myBookDataStorage.CreateAsync(book, cancellationToken);

			return book;
		}

		public async Task<MyBook> HandleAsync(AppendNewBookToMyBookList request, CancellationToken cancellationToken)
		{
			var book = new MyBook(request.Author, request.Title);

			book = await _myBookDataStorage.CreateAsync(book, cancellationToken);

			return book;
		}

		public async Task<IEnumerable<MyBook>> HandleAsync(AppendSharedBookListToMyBookList request,
			CancellationToken cancellationToken)
		{
			var sharedList = await _queryDispatcher.FetchAsync(new SharedBookListQuery(request.SharedBookListKey),
				cancellationToken);

			var addedMyBooks = new List<MyBook>();
			foreach (var myBook in sharedList.Books.Select(n => new MyBook(n)).ToList())
			{
				var addedMyBook = await _myBookDataStorage.CreateAsync(myBook, cancellationToken);

				addedMyBooks.Add(addedMyBook);
			}

			return addedMyBooks;
		}

		public async Task<None> HandleAsync(UpdateMyBookReview request, CancellationToken cancellationToken)
		{
			var myBook = await _myBookDataStorage.GetAsync(request.Key, cancellationToken);

			myBook.UpdateReview(request.Review);
			await _myBookDataStorage.UpdateAsync(myBook, cancellationToken);

			return None.Value;
		}

		public async Task<None> HandleAsync(UpdateMyBookStatus request, CancellationToken cancellationToken)
		{
			var myBook = await _myBookDataStorage.GetAsync(request.Key, cancellationToken);

			myBook.UpdateStatus(request.Status);
			await _myBookDataStorage.UpdateAsync(myBook, cancellationToken);

			return None.Value;
		}

		public async Task<None> HandleAsync(UpdateMyBookOrdering request, CancellationToken cancellationToken)
		{
			var myBook = await _myBookDataStorage.GetAsync(request.Key, cancellationToken);

			myBook.UpdateOrdering(request.Ordering);
			await _myBookDataStorage.UpdateAsync(myBook, cancellationToken);

			return None.Value;
		}
	}
}