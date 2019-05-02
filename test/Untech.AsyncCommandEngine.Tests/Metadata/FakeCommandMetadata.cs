using Untech.AsyncCommandEngine.Metadata.Annotations;

namespace Untech.AsyncCommandEngine.Metadata
{
	[WatchDogTimeout(0, 0, 10)]
	public class FakeCommandMetadata : IRequestMetadataSource<FakeCommand>
	{

	}
}