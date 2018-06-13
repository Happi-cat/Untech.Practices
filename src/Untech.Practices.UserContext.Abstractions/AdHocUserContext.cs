using System;
using System.Collections.Generic;

namespace Untech.Practices.UserContext
{
	/// <summary>
	/// Basic <see cref="IUserContext{TKey}"/>.
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	public class AdHocUserContext<TKey> : IUserContext<TKey> where TKey: IEquatable<TKey>
	{
		private readonly IReadOnlyDictionary<string, string> _options;

		/// <summary>
		/// Initializes a <see cref="AdHocUserContext{TKey}"/> with a predefined <paramref name="userKey"/>
		/// and optional user <paramref name="options"/>.
		/// </summary>
		/// <param name="userKey">The current user key.</param>
		/// <param name="options">The current user options.</param>
		public AdHocUserContext(TKey userKey, IReadOnlyDictionary<string, string> options = null)
		{
			_options = options;
			UserKey = userKey;
		}


		/// <inheritdoc />
		public TKey UserKey { get; }

		/// <inheritdoc />
		public string this[string key]
		{
			get
			{
				if (_options != null && _options.TryGetValue(key, out var value))
				{
					return value;
				}

				return null;
			}
		}
	}

	/// <summary>
	/// Basic <see cref="IUserContext"/>.
	/// </summary>
	public class AdHocUserContext : AdHocUserContext<int>, IUserContext
	{
		/// <summary>
		/// Initializes a <see cref="AdHocUserContext{TKey}"/> with a predefined <paramref name="userKey"/>
		/// and optional user <paramref name="options"/>.
		/// </summary>
		/// <param name="userKey">The current user key.</param>
		/// <param name="options">The current user options.</param>
		public AdHocUserContext(int userKey, IReadOnlyDictionary<string, string> options = null)
			:base(userKey, options)
		{

		}
	}
}