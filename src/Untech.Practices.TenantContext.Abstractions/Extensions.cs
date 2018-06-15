﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.Practices.TenantContext
{
	/// <summary>
	/// Defines a set of extension-methods that use <see cref="ITenantContext{TKey}"/>.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Returns elements from original sequence that belong to user from <paramref name="tenantContext"/>.
		/// </summary>
		/// <param name="source">The original source.</param>
		/// <param name="tenantContext">The user context.</param>
		/// <typeparam name="TElem">The type of sequence elements.</typeparam>
		/// <typeparam name="TTenantKey">The type of tenant key.</typeparam>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">
		/// Thrown when <paramref name="source"/> or <paramref name="tenantContext"/> is null.
		/// </exception>
		public static IQueryable<TElem> TakeTenantSpecific<TElem, TTenantKey>(this IQueryable<TElem> source,
			ITenantContext<TTenantKey> tenantContext)
			where TElem : IHaveTenantKey<TTenantKey>
			where TTenantKey : IEquatable<TTenantKey>
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (tenantContext == null) throw new ArgumentNullException(nameof(tenantContext));

			var tenantKeyToMatch = tenantContext.TenantKey;

			return source.Where(n => tenantKeyToMatch.Equals(n.TenantKey));
		}

		/// <summary>
		/// Returns elements from original sequence that belong to user from <paramref name="tenantContext"/>.
		/// </summary>
		/// <param name="source">The original source.</param>
		/// <param name="tenantContext">The user context.</param>
		/// <typeparam name="TElem">The type of sequence elements.</typeparam>
		/// <typeparam name="TTenantKey">The type of tenant key.</typeparam>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">
		/// Thrown when <paramref name="source"/> or <paramref name="tenantContext"/> is null.
		/// </exception>
		public static IEnumerable<TElem> TakeTenantSpecific<TElem, TTenantKey>(this IEnumerable<TElem> source,
			ITenantContext<TTenantKey> tenantContext)
			where TElem : IHaveTenantKey<TTenantKey>
			where TTenantKey : IEquatable<TTenantKey>
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (tenantContext == null) throw new ArgumentNullException(nameof(tenantContext));

			var tenantKeyToMatch = tenantContext.TenantKey;

			return source.Where(n => tenantKeyToMatch.Equals(n.TenantKey));
		}
	}
}