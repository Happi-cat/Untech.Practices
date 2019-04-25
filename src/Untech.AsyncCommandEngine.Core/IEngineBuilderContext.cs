using Microsoft.Extensions.Logging;
using Untech.AsyncCommandEngine.Metadata;
using Untech.AsyncCommandEngine.Transports;

namespace Untech.AsyncCommandEngine
{
	public interface IEngineBuilderContext
	{
		ILoggerFactory GetLogger();
		ITransport GetTransport();
		IRequestMetadataProvider GetMetadata();
	}
}