using System;

namespace Untech.Practices.DataStorage
{
	/// <summary>
	///     Represents errors that occur when <see cref="IAggregateRoot{TKey}" /> cannot be found.
	/// </summary>
	public class AggregateRootNotFoundException : Exception
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="AggregateRootNotFoundException" /> class.
		/// </summary>
		/// <param name="key">The key of <see cref="IAggregateRoot{TKey}" /> that was not found.</param>
		/// <param name="message">Error message.</param>
		public AggregateRootNotFoundException(object key, string message = null)
			: base(GetMessage(key, message))
		{
			Key = key;
		}

		/// <summary>
		///     Gets the key of the <see cref="IAggregateRoot{TKey}" /> that was not found.
		/// </summary>
		public object Key { get; }

		private static string GetMessage(object key, string message = null)
		{
			return string.IsNullOrEmpty(message)
				? $"Aggregate root with '{key}' was not found."
				: message;
		}
	}
}