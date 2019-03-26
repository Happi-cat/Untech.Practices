namespace Untech.AsyncCommandEngine.Metadata
{
	public sealed class NullRequestMetadataAccessors : IRequestMetadataAccessors
	{
		public static readonly IRequestMetadataAccessors Default = new NullRequestMetadataAccessors();

		private NullRequestMetadataAccessors()
		{

		}

		public IRequestMetadataAccessor GetMetadata(string requestName)
		{
			return NullRequestMetadataAccessor.Default;
		}
	}
}