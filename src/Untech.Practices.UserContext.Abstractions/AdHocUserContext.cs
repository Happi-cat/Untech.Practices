using System;
using System.Collections.Generic;

namespace Untech.Practices.UserContext
{
	public class AdHocUserContext<TKey> : IUserContext<TKey> where TKey: IEquatable<TKey>
	{
		private readonly IReadOnlyDictionary<string, string> _options;

		public AdHocUserContext(TKey userKey, IReadOnlyDictionary<string, string> options = null)
		{
			_options = options;
			UserKey = userKey;
		}

		public TKey UserKey { get; }

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

	public class AdHocUserContext : AdHocUserContext<int>, IUserContext
	{
		public AdHocUserContext(int userKey, IReadOnlyDictionary<string, string> options = null)
			:base(userKey, options)
		{

		}
	}
}