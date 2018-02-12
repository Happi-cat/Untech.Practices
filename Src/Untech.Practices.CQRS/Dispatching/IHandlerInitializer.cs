namespace Untech.Practices.CQRS.Dispatching
{
	public interface IHandlerInitializer
	{
		/// <summary>
		/// Initializes the specified handler.
		/// </summary>
		/// <param name="handler">The handler that should be initialized.</param>
		/// <param name="request">The request that was received and should be processed.</param>
		void Init(object handler, object request);
	}
}