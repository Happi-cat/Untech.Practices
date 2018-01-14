namespace Untech.Practices.CQRS.Pipeline
{
	public interface IPipelinePostProcessor<in TRequest, in TResponse>
	{
		void Process(TRequest request, TResponse response);
	}
}