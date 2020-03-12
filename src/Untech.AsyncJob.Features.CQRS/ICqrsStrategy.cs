using Untech.AsyncJob.Formatting;
using Untech.Practices.CQRS.Dispatching;

namespace Untech.AsyncJob.Features.CQRS
{
	public interface ICqrsStrategy
	{
		IRequestTypeFinder GetRequestTypeFinder();

		IDispatcher GetDispatcher(Context context);

		IRequestContentFormatter GetRequestFormatter(Context context);
	}
}
