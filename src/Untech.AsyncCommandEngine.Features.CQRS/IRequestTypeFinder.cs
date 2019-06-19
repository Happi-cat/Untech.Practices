using System;

namespace Untech.AsyncCommandEngine.Features.CQRS
{
	public interface IRequestTypeFinder
	{
		Type FindRequestType(string requestName);
	}
}