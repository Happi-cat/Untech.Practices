using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.Practices.UserContext
{
	public static class Extensions
	{
		public static IQueryable<T> TakeMineOnly<T, TKey>(this IQueryable<T> queryable,
			IUserContext<TKey> userContext)
			where T : IHaveUserKey<TKey>
			where TKey : IEquatable<TKey>
		{
			if (queryable == null) throw new ArgumentNullException(nameof(queryable));
			if (userContext == null) throw new ArgumentNullException(nameof(userContext));

			return queryable.Where(n => userContext.UserKey.Equals(n.UserKey));
		}

		public static IEnumerable<T> TakeMineOnly<T, TKey>(this IEnumerable<T> queryable,
			IUserContext<TKey> userContext)
			where T : IHaveUserKey<TKey>
			where TKey : IEquatable<TKey>
		{
			if (queryable == null) throw new ArgumentNullException(nameof(queryable));
			if (userContext == null) throw new ArgumentNullException(nameof(userContext));

			return queryable.Where(n => userContext.UserKey.Equals(n.UserKey));
		}
	}
}