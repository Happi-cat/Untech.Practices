using System;

namespace Untech.Practices.UserContext
{
	public interface IHaveUserKey : IHaveUserKey<int>
	{

	}

	public interface IHaveUserKey<out TKey> where TKey: IEquatable<TKey>
	{
		TKey UserKey { get; }
	}
}