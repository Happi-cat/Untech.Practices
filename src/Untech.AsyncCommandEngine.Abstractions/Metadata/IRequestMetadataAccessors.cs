namespace Untech.AsyncCommandEngine.Metadata
{
	public interface IRequestMetadataAccessors
	{
		IRequestMetadataAccessor GetMetadata(string requestName);
	}
}