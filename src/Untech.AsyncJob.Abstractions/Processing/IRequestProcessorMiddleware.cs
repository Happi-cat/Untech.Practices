using System.Threading.Tasks;

namespace Untech.AsyncJob.Processing
{
	/// <summary>
	/// Represents interfaces that defines methods of step from <see cref="IRequestProcessor"/> pipeline.
	/// </summary>
	public interface IRequestProcessorMiddleware
	{
		/// <summary>
		/// Process request <see cref="Context"/> asynchronously
		/// and calls <paramref name="next"/> pipeline step if required.
		/// </summary>
		/// <param name="context">The context with current request to process.</param>
		/// <param name="next">The next step to be invoked.</param>
		/// <returns>The Task to await.</returns>
		Task InvokeAsync(Context context, RequestProcessorCallback next);
	}
}
