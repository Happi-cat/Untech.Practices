using Microsoft.Extensions.Logging;

namespace Untech.AsyncJob.Builder
{
	public interface IBuilderContext
	{
		/// <summary>
		/// Gets the instance of <see cref="ILoggerFactory"/> that was registered.
		/// </summary>
		/// <returns></returns>
		ILoggerFactory GetLogger();
	}
}
