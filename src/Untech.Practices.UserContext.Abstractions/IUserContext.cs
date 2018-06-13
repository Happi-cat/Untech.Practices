using System;

namespace Untech.Practices.UserContext
{
	/// <summary>
	/// Describes user context infomation.
	/// Can be used when user key is of <see cref="Int32"/> type.
	/// </summary>
	public interface IUserContext : IUserContext<int>
	{
	}

	/// <summary>
	/// Describes user context inforamtion.
	/// </summary>
	/// <typeparam name="TKey">The type of user key.</typeparam>
	public interface IUserContext<out TKey> where TKey: IEquatable<TKey>
	{
		/// <summary>
		/// Gets key of the current user.
		/// </summary>
		TKey UserKey { get; }

		/// <summary>
		/// Gets the value corresponding to a user option key.
		/// </summary>
		/// <param name="key">The user option key.</param>
		string this[string key] { get; }
	}
}