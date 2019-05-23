using Microsoft.Extensions.Logging;
using Untech.AsyncCommandEngine.Metadata;
using Untech.AsyncCommandEngine.Transports;

namespace Untech.AsyncCommandEngine.Builder
{
	public interface IBuilderContext
	{
		/// <summary>
		/// Gets the instance of <see cref="ILoggerFactory"/> that was registered.
		/// </summary>
		/// <returns></returns>
		ILoggerFactory GetLogger();

		/// <summary>
		/// Gets the instance of <see cref="ITransport"/> that was registered.
		/// </summary>
		/// <returns></returns>
		ITransport GetTransport();

		/// <summary>
		/// Gets the instance of <see cref="IRequestMetadataProvider"/> that was registered.
		/// </summary>
		/// <returns></returns>
		IRequestMetadataProvider GetMetadata();
	}
}