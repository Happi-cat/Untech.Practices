namespace Untech.AsyncJob.Metadata
{
	/// <summary>
	/// Represents dummy <see cref="IRequestMetadataProvider"/>.
	/// </summary>
	public sealed class NullRequestMetadataProvider : IRequestMetadataProvider
	{
		/// Gets the shared instance of <see cref="NullRequestMetadataProvider"/>.
		public static readonly IRequestMetadataProvider Instance = new NullRequestMetadataProvider();

		/// <inheritdoc />
		public IRequestMetadata GetMetadata(string requestName)
		{
			return NullRequestMetadata.Instance;
		}
	}
}
