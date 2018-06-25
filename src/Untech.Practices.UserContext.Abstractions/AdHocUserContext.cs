using System;
using System.Collections.Generic;

namespace Untech.Practices.UserContext
{
	/// <summary>
	/// Basic <see cref="IUserContext{TKey}"/>.
	/// </summary>
	/// <typeparam name="TKey">The type of user key. Should implement <see cref="IEquatable{T}"/>.</typeparam>
	public class AdHocUserContext<TKey> : IUserContext<TKey> where TKey: IEquatable<TKey>
	{
		private readonly IReadOnlyDictionary<string, string> _options;

		/// <summary>
		/// Initializes a new instance of <see cref="AdHocUserContext{TKey}"/> with a predefined <paramref name="userKey"/>
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
		public string this[string optionKey]
		{
			get
			{
				if (_options != null && _options.TryGetValue(optionKey, out var value))
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