namespace Untech.Practices.CQRS.Pipeline
{
	public interface IPipelinePreProcessor<in TRequest>
	{
		void Process(TRequest request);
	}
}