using System;
using System.Globalization;

namespace Untech.Practices.UserContext
{
	/// <summary>
	///     Describes user context infomation.
	///     Can be used when user key is of <see cref="int" /> type.
	/// </summary>
	public interface IUserContext : IUserContext<int>
	{
	}

	/// <summary>
	///     Describes user context inforamtion.
	/// </summary>
	/// <typeparam name="TKey">The type of user key. Should implement <see cref="IEquatable{T}" />.</typeparam>
	public interface IUserContext<out TKey> where TKey : IEquatable<TKey>
	{
		/// <summary>
		///     Gets key of the current user.
		/// </summary>
		TKey UserKey { get; }

		/// <summary>
		///     Gets user's culture.
		/// </summary>
		CultureInfo Culture { get; }

		/// <summary>
		///     Gets the value corresponding to a user option key.
		/// </summary>
		/// <param name="optionKey">The user option key.</param>
		string this[string optionKey] { get; }
	}
}