using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.Serialization;
using MyBookList.Domain.Books;
using Untech.Practices.CQRS;
using Untech.Practices.DataStorage;

namespace MyBookList.Domain.BookLists.My
{
	[DataContract]
	public class MyBook : IAggregateRoot
	{
		private MyBook()
		{

		}

		public MyBook(string author, string title, IEnumerable<string> tags,int? ordering = null,  string notes = null)
			: this (0, null, author, title, MyBookStatus.Pending,  ordering, notes)
		{

		}

		public MyBook(Book book, int? ordering = null,  string notes = null)
			: this (0, book.Key, book.Author, book.Title, MyBookStatus.Pending, ordering, notes)
		{

		}

		public MyBook(int key, int? bookKey, string author, string title, MyBookStatus status, int? ordering = null, string notes = null)
		{
			Key = key;
			BookKey = bookKey;
			Ordering = ordering;
			Author = author;
			Title = title;
			Status = status;
			Notes = notes;
		}

		[DataMember]
		public int Key { get; private set; }

		[DataMember]
		public int? BookKey { get; private set; }

		[DataMember]
		public int? Ordering { get; private set; }

		[DataMember]
		public string Author { get; private set; }

		[DataMember]
		public string Title { get; private set; }

		[DataMember]
		public string Notes { get; private set; }

		[DataMember]
		public MyBookStatus Status { get; private set; }

		public void UpdateStatus(MyBookStatus status)
		{
			Status = status;
		}

		public void UpdateNotes(string notes)
		{
			Notes = notes;
		}

		public void UpdateOrdering(int? ordering)
		{
			Ordering = ordering;
		}
	}

	public class AddNewBookToMyBookList : ICommand
	{

	}

	public class AddExistingBookToMyBookList : ICommand
	{

	}

	public class AddSharedListBooksToMyBookList : ICommand
	{

	}

	public class UpdateMyBookStatus : ICommand { }

	public class UpdateMyBookOrdering : ICommand
	{

	}

	public class UpdateMyBookNote : ICommand
	{

	}

	public class MyBookListQuery : IQuery<MyBookList>
	{

	}

	public class NextBooksToReadQuery : IQuery<IEnumerable<MyBook>>
	{
		public bool GetLucky { get; private set; }
	}
}