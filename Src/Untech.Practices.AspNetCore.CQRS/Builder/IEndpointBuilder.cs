namespace Untech.Practices.AspNetCore.CQRS.Builder
{
	public interface IEndpointBuilder
	{
		IEndpointBuilder MapVerb<TIn, TOut>(string verb, CqrsHandler<TIn, TOut> handler);
	}
}
