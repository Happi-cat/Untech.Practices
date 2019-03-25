using System.Reflection;
using Untech.Practices.CQRS.Dispatching;

namespace Untech.AsyncCommandEngine.Processing
{
	public interface ICqrsStrategy
	{
		TypeInfo FindRequestType(string requestName);

		object MaterializeRequest(Request request);

		IDispatcher GetDispatcher(Context context);
	}
}