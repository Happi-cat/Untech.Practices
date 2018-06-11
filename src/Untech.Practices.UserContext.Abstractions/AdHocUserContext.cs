using System;

namespace Untech.Practices.UserContext
{
	public class AdHocUserContext<TKey> : IUserContext<TKey> where TKey: IEquatable<TKey>
	{
		public AdHocUserContext(TKey userKey)
		{
			UserKey = userKey;
		}

		public TKey UserKey { get; }
	}

	public class AdHocUserContext : AdHocUserContext<int>, IUserContext
	{
		public AdHocUserContext(int userKey)
			:base(userKey)
		{

		}
	}
}