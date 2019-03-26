using System;
using Untech.Practices.CQRS.Dispatching;

namespace Untech.AsyncCommandEngine.Processing
{
	public interface ICqrsStrategy
	{
		Type FindRequestType(string requestName);

		object MaterializeRequest(Request request);

		IDispatcher GetDispatcher(Context context);
	}
}