namespace Untech.AsyncJob.Metadata
{
	/// <summary>
	/// Represents interface that contains methods for metadata loading for the current <see cref="Request"/>.
	/// </summary>
	public interface IRequestMetadataProvider
	{
		/// <summary>
		/// Gets <see cref="IRequestMetadata"/> for the specified <paramref name="request"/>.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>Instance of the <see cref="IRequestMetadata"/>.</returns>
		IRequestMetadata GetMetadata(Request request);
	}
}
