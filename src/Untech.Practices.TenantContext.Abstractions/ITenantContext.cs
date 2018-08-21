using System;

namespace Untech.Practices.TenantContext
{
	/// <summary>
	/// Describes tenant context information.
	/// </summary>
	/// <typeparam name="TKey">The type of tenant key. Should implement <see cref="IEquatable{T}"/>.</typeparam>
	public interface ITenantContext<out TKey>
		where TKey : IEquatable<TKey>
	{
		/// <summary>
		/// Gets key of the current tenant.
		/// </summary>
		TKey TenantKey { get; }

		/// <summary>
		/// Gets the value corresponding to a tenant option key.
		/// </summary>
		/// <param name="optionKey">The tenant option key.</param>
		string this[string optionKey] { get; }
	}

	/// <summary>
	/// Describes tenant context information.
	/// Can be used when tenant key is of <see cref="Int32"/> type.
	/// </summary>
	public interface ITenantContext : ITenantContext<int>
	{

	}
}