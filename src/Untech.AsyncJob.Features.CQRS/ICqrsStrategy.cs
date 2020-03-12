using Untech.AsyncJob.Formatting;
using Untech.Practices.CQRS.Dispatching;

namespace Untech.AsyncJob.Features.CQRS
{
	public interface ICqrsStrategy : IRequestTypeFinder
	{
		IDispatcher GetDispatcher(Context context);

		IRequestContentFormatter GetRequestFormatter(Context context);
	}
}
