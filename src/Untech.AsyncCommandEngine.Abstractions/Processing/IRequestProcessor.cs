using System.Threading.Tasks;

namespace Untech.AsyncCommandEngine.Processing
{
	/// <summary>
	/// Represents interfaces that provides method for <see cref="Context"/> processing.
	/// </summary>
	public interface IRequestProcessor
	{
		/// <summary>
		/// Process request <paramref name="context"/> asynchronously.
		/// </summary>
		/// <param name="context">The context with current request.</param>
		/// <returns>Task to await.</returns>
		Task InvokeAsync(Context context);
	}
}