using System.Collections.Generic;
using MyBookList.Domain.BookLists.My;
using Untech.Practices.CQRS;

namespace MyBookList.Infrastructure.Data
{
	public class MyBooksQuery : IQuery<IEnumerable<MyBook>>
	{
		private IReadOnlyList<MyBookStatus> _statuses;

		public IReadOnlyList<MyBookStatus> OnlyStatuses
		{
			get => _statuses ?? (_statuses = new List<MyBookStatus>());
			set => _statuses = value;
		}
	}
}