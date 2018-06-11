using System;

namespace Untech.Practices.UserContext
{
	public interface IUserContext : IUserContext<int>
	{
	}

	public interface IUserContext<out TKey> where TKey: IEquatable<TKey>
	{
		TKey UserKey { get; }
	}
}