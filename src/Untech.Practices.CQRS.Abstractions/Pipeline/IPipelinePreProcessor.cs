using Untech.Practices.CQRS.Handlers;

namespace Untech.Practices.CQRS.Pipeline
{
	/// <summary>
	///     Defines a pipeline step that will be triggered beefore <see cref="IRequestHandler{TIn,TOut}" /> or
	///     <see cref="IRequestHandler{TIn,TOut}" />
	/// </summary>
	/// <typeparam name="TRequest">CQRS request type.</typeparam>
	public interface IPipelinePreProcessor<in TRequest>
	{
		/// <summary>
		///     Pre process <paramref name="request" />.
		/// </summary>
		/// <param name="request">CQRS request.</param>
		void Process(TRequest request);
	}
}