using System;
using System.Collections.Generic;

namespace Untech.Practices.TenantContext
{
	/// <summary>
	///     Basic <see cref="ITenantContext{TKey}" />.
	/// </summary>
	/// <typeparam name="TKey">The type of tenant key. Should implement <see cref="IEquatable{T}" />.</typeparam>
	public class AdHocTenantContext<TKey> : ITenantContext<TKey>
		where TKey : IEquatable<TKey>
	{
		private readonly IReadOnlyDictionary<string, string> _options;

		/// <summary>
		///     Initializes a new instance with a predefined <paramref name="tenantKey" />
		///     and optional set of tenant <paramref name="options" />.
		/// </summary>
		/// <param name="tenantKey">The current tenant key to use.</param>
		/// <param name="options">The current tenant options to use.</param>
		public AdHocTenantContext(TKey tenantKey, IReadOnlyDictionary<string, string> options = null)
		{
			_options = options;
			TenantKey = tenantKey;
		}

		/// <inheritdoc />
		public TKey TenantKey { get; }

		/// <inheritdoc />
		public string this[string optionKey]
		{
			get
			{
				if (_options != null && _options.TryGetValue(optionKey, out string value)) return value;

				return null;
			}
		}
	}

	/// <summary>
	///     Basic <see cref="ITenantContext{TKey}" />.
	/// </summary>
	public class AdHocTenantContext : AdHocTenantContext<int>, ITenantContext
	{
		/// <summary>
		///     Initializes a new instance with a predefined <paramref name="tenantKey" />
		///     and optional set of tenant <paramref name="options" />.
		/// </summary>
		/// <param name="tenantKey">The current tenant key to use.</param>
		/// <param name="options">The current tenant options to use.</param>
		public AdHocTenantContext(int tenantKey, IReadOnlyDictionary<string, string> options = null)
			: base(tenantKey, options)
		{
		}
	}
}