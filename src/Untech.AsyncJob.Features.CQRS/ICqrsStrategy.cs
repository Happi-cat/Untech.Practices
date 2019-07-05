using Untech.Practices.CQRS.Dispatching;

namespace Untech.AsyncJob.Features.CQRS
{
	public interface ICqrsStrategy : IRequestTypeFinder
	{
		IDispatcher GetDispatcher(Context context);
	}
}
