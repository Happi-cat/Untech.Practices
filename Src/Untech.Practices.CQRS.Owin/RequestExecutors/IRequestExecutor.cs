using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Untech.Practices.CQRS.Dispatching;

namespace Untech.Practices.CQRS.Owin.RequestExecutors
{
	public interface IRequestExecutor
	{
		Type RequestType { get; }

		Type ResponseType { get; }

		Task Handle(IOwinContext context, IDispatcher dispatcher);
	}
}