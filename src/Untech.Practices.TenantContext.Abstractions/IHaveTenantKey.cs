using System;

namespace Untech.Practices.TenantContext
{
	/// <summary>
	///     Marks tenant-related models.
	/// </summary>
	/// <typeparam name="TKey">The type of tenant key. Should implement <see cref="IEquatable{T}" />.</typeparam>
	public interface IHaveTenantKey<out TKey>
		where TKey : IEquatable<TKey>
	{
		/// <summary>
		///     Gets key of the current tenant.
		/// </summary>
		TKey TenantKey { get; }
	}

	/// <summary>
	///     Marks tenant-related models.
	///     Can be used when tenant key is of <see cref="Int32" /> type.
	/// </summary>
	public interface IHaveTenantKey : IHaveTenantKey<int>
	{
	}
}