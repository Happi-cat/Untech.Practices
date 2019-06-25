namespace Untech.Practices.TenantContext
{
	/// <summary>
	/// Describes tenant context information.
	/// </summary>
	public interface ITenantContext
	{
		/// <summary>
		/// Gets key of the current tenant.
		/// </summary>
		string TenantKey { get; }

		/// <summary>
		/// Gets the value corresponding to a tenant option key.
		/// </summary>
		/// <param name="optionKey">The tenant option key.</param>
		object this[string optionKey] { get; }
	}
}