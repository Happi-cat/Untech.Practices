namespace Untech.AsyncCommandEngine.Metadata
{
	public interface IRequestMetadataProvider
	{
		IRequestMetadata GetMetadata(string requestName);
	}
}