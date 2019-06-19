using Untech.Practices.CQRS.Dispatching;

namespace Untech.AsyncCommandEngine.Features.CQRS
{
	public interface ICqrsStrategy : IRequestTypeFinder
	{
		IDispatcher GetDispatcher(Context context);
	}
}