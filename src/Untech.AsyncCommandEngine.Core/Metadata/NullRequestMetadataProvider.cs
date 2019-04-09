namespace Untech.AsyncCommandEngine.Metadata
{
	public sealed class NullRequestMetadataProvider : IRequestMetadataProvider
	{
		public static readonly IRequestMetadataProvider Instance = new NullRequestMetadataProvider();

		private NullRequestMetadataProvider()
		{

		}

		public IRequestMetadata GetMetadata(string requestName)
		{
			return NullRequestMetadata.Instance;
		}
	}
}