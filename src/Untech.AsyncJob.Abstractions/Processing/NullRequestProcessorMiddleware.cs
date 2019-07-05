using System.Threading.Tasks;

namespace Untech.AsyncJob.Processing
{
	/// <summary>
	/// Represents dummy <see cref="IRequestProcessorMiddleware"/>.
	/// </summary>
	public sealed class NullRequestProcessorMiddleware : IRequestProcessorMiddleware
	{
		/// <summary>
		/// Gets the shared instance of the <see cref="NullRequestProcessorMiddleware"/>.
		/// </summary>
		public static readonly IRequestProcessorMiddleware Instance = new NullRequestProcessorMiddleware();

		/// <inheritdoc />
		public Task InvokeAsync(Context context, RequestProcessorCallback next)
		{
			return Task.FromResult(0);
		}
	}
}
