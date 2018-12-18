using System;
using System.Runtime.Serialization;
using Untech.Practices.DataStorage;

namespace MyBudgetPlan.Domain.ExpenseLog.Category
{
	[DataContract]
	public class ExpenseCategory : IAggregateRoot<string>
	{
		private const string Delimiter = ":";

		private ExpenseCategory()
		{
		}

		public ExpenseCategory(string key, string title, string description = null)
		{
			Key = key;
			Title = title;
			Description = description;
		}

		[DataMember]
		public string Key { get; private set; }

		[DataMember]
		public string Title { get; private set; }

		[DataMember]
		public string ParentKey
		{
			get
			{
				var indexOfDelimiter = Key.LastIndexOf(Delimiter, StringComparison.OrdinalIgnoreCase);

				return indexOfDelimiter > -1
					? Key.Substring(0, indexOfDelimiter)
					: string.Empty;
			}
		}

		[DataMember]
		public string Description { get; private set; }

		public bool IsSameOrDescendantOf(string parentKey)
		{
			return string.Equals(Key, parentKey) || Key.StartsWith(parentKey + Delimiter);
		}

		public bool IsSameOrParentOf(string childKey)
		{
			return string.Equals(Key, childKey) || childKey.StartsWith(Key + Delimiter);
		}
	}
}