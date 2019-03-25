namespace Untech.AsyncCommandEngine.Metadata
{
	public class NullRequestMetadataAccessors : IRequestMetadataAccessors
	{
		public static readonly IRequestMetadataAccessors Default = new NullRequestMetadataAccessors();

		public IRequestMetadataAccessor GetMetadata(string requestName)
		{
			return NullRequestMetadataAccessor.Default;
		}
	}
}