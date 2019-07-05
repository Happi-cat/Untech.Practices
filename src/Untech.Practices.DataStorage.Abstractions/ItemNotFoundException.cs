using System;

namespace Untech.Practices.DataStorage
{
	/// <summary>
	///     Represents errors that occur when <see cref="IHasKey{TKey}" /> cannot be found.
	/// </summary>
	public class ItemNotFoundException : Exception
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="ItemNotFoundException" /> class.
		/// </summary>
		/// <param name="key">The key of <see cref="IHasKey{TKey}" /> that was not found.</param>
		/// <param name="message">Error message.</param>
		public ItemNotFoundException(object key, string message = null)
			: base(GetMessage(key, message))
		{
			Key = key;
		}

		/// <summary>
		///     Gets the key of the <see cref="IHasKey{TKey}" /> that was not found.
		/// </summary>
		public object Key { get; }

		private static string GetMessage(object key, string message = null)
		{
			return string.IsNullOrEmpty(message)
				? $"Item with '{key}' was not found."
				: message;
		}
	}
}