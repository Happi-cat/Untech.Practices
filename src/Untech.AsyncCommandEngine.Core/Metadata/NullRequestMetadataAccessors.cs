namespace Untech.AsyncCommandEngine.Metadata
{
	public sealed class NullRequestMetadataAccessors : IRequestMetadataAccessors
	{
		public static readonly IRequestMetadataAccessors Instance = new NullRequestMetadataAccessors();

		private NullRequestMetadataAccessors()
		{

		}

		public IRequestMetadataAccessor GetMetadata(string requestName)
		{
			return NullRequestMetadataAccessor.Instance;
		}
	}
}