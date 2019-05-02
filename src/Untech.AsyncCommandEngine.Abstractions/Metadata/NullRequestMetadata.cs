using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Untech.AsyncCommandEngine.Metadata
{
	/// <summary>
	/// Represents dummy <see cref="IRequestMetadata"/>.
	/// </summary>
	public sealed class NullRequestMetadata : IRequestMetadata
	{
		/// <summary>
		/// Gets the shared instance of <see cref="NullRequestMetadata"/>.
		/// </summary>
		public static readonly IRequestMetadata Instance = new NullRequestMetadata();

		/// <inheritdoc />
		public TAttr GetAttribute<TAttr>() where TAttr : Attribute
		{
			return default;
		}

		/// <inheritdoc />
		public IEnumerable<TAttr> GetAttributes<TAttr>() where TAttr : Attribute
		{
			return Enumerable.Empty<TAttr>();
		}
	}
}