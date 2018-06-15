using System;

namespace Untech.Practices.UserContext
{
	/// <summary>
	/// Marks user-related models.
	/// Can be used when user key is of <see cref="Int32"/> type.
	/// </summary>
	public interface IHaveUserKey : IHaveUserKey<int>
	{

	}

	/// <summary>
	/// Marks user-related models.
	/// </summary>
	/// <typeparam name="TKey">The type of user key. Should implement <see cref="IEquatable{T}"/>.</typeparam>
	public interface IHaveUserKey<out TKey>
		where TKey: IEquatable<TKey>
	{
		/// <summary>
		/// Gets key of the user associated with current object.
		/// </summary>
		TKey UserKey { get; }
	}
}