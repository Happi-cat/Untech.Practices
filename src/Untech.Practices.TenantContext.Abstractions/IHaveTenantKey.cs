using System;

namespace Untech.Practices.TenantContext
{
	/// <summary>
	///     Marks tenant-related models.
	/// </summary>
	public interface IHaveTenantKey
	{
		/// <summary>
		///     Gets key of the current tenant.
		/// </summary>
		string TenantKey { get; }
	}
}