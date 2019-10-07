using System.Threading.Tasks;

namespace Untech.AsyncJob.Processing
{
	/// <summary>
	/// Represents callback that should be invoked as a part of <see cref="IRequestProcessorMiddleware"/>.
	/// </summary>
	/// <param name="context"></param>
	public delegate Task RequestProcessorCallback(Context context);
}
