using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.Practices.UserContext
{
	/// <summary>
	/// Defines a set of extension-methods that use <see cref="IUserContext{TKey}"/>.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Returns elements from original sequence that belong to user from <paramref name="userContext"/>.
		/// </summary>
		/// <param name="source">The original source.</param>
		/// <param name="userContext">The user context.</param>
		/// <typeparam name="TElem">The type of sequence elements.</typeparam>
		/// <typeparam name="TUserKey">The type of user key.</typeparam>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">
		/// Thrown when <paramref name="source"/> or <paramref name="userContext"/> is null.
		/// </exception>
		public static IQueryable<TElem> TakeMineOnly<TElem, TUserKey>(this IQueryable<TElem> source,
			IUserContext<TUserKey> userContext)
			where TElem : IHaveUserKey<TUserKey>
			where TUserKey : IEquatable<TUserKey>
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));
			if (userContext == null)
				throw new ArgumentNullException(nameof(userContext));

			var userKeyToMatch = userContext.UserKey;

			return source.Where(n => userKeyToMatch.Equals(n.UserKey));
		}

		/// <summary>
		/// Returns elements from original sequence that belong to user from <paramref name="userContext"/>.
		/// </summary>
		/// <param name="source">The original source.</param>
		/// <param name="userContext">The user context.</param>
		/// <typeparam name="TElem">The type of sequence elements.</typeparam>
		/// <typeparam name="TUserKey">The type of user key.</typeparam>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">
		/// Thrown when <paramref name="source"/> or <paramref name="userContext"/> is null.
		/// </exception>
		public static IEnumerable<TElem> TakeMineOnly<TElem, TUserKey>(this IEnumerable<TElem> source,
			IUserContext<TUserKey> userContext)
			where TElem : IHaveUserKey<TUserKey>
			where TUserKey : IEquatable<TUserKey>
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));
			if (userContext == null)
				throw new ArgumentNullException(nameof(userContext));

			var userKeyToMatch = userContext.UserKey;

			return source.Where(n => userKeyToMatch.Equals(n.UserKey));
		}
	}
}