using System.Collections.Generic;

namespace Untech.Practices.TenantContext
{
	/// <summary>
	///     Basic <see cref="ITenantContext{TKey}" />.
	/// </summary>
	public class AdHocTenantContext<TKey> : ITenantContext
	{
		private readonly IReadOnlyDictionary<string, object> _options;

		/// <summary>
		///     Initializes a new instance with a predefined <paramref name="tenantKey" />
		///     and optional set of tenant <paramref name="options" />.
		/// </summary>
		/// <param name="tenantKey">The current tenant key to use.</param>
		/// <param name="options">The current tenant options to use.</param>
		public AdHocTenantContext(string tenantKey, IReadOnlyDictionary<string, object> options = null)
		{
			_options = options;
			TenantKey = tenantKey;
		}

		/// <inheritdoc />
		public string TenantKey { get; }

		/// <inheritdoc />
		public object this[string optionKey]
		{
			get
			{
				if (_options != null && _options.TryGetValue(optionKey, out object value))
					return value;

				return null;
			}
		}
	}
}