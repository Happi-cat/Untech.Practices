using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.Serialization;
using MyBookList.Domain.Library;
using Untech.Practices.DataStorage;

namespace MyBookList.Domain.BookLists.My
{
	[DataContract]
	public class MyBook : IHasKey
	{
		private MyBook()
		{

		}

		public MyBook(string author, string title, int? ordering = null, string notes = null)
			: this(0, null, author, title, MyBookStatus.Pending, ordering, notes)
		{

		}

		public MyBook(Book book, int? ordering = null, string notes = null)
			: this(0, book.Key, book.Author, book.Title, MyBookStatus.Pending, ordering, notes)
		{

		}

		public MyBook(int key, int? bookKey, string author, string title, MyBookStatus status, int? ordering = null, string review = null)
		{
			Key = key;
			BookKey = bookKey;
			Ordering = ordering;
			Author = author;
			Title = title;
			Status = status;
			Review = review;
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
		public string Review { get; private set; }

		[DataMember]
		public MyBookStatus Status { get; private set; }

		public void UpdateStatus(MyBookStatus status)
		{
			Status = status;
		}

		public void UpdateReview(string review)
		{
			Review = review;
		}

		public void UpdateOrdering(int? ordering)
		{
			Ordering = ordering;
		}
	}
}