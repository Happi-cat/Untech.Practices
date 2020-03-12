using System.Collections.Generic;
using System.Linq;
using Untech.AsyncJob.Metadata.Annotations;

namespace Untech.AsyncJob.Metadata
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
		public TAttr GetAttribute<TAttr>() where TAttr : MetadataAttribute
		{
			return default;
		}

		/// <inheritdoc />
		public IEnumerable<TAttr> GetAttributes<TAttr>() where TAttr : MetadataAttribute
		{
			return Enumerable.Empty<TAttr>();
		}
	}
}
