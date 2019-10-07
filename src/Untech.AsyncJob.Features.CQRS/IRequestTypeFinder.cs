using System;

namespace Untech.AsyncJob.Features.CQRS
{
	public interface IRequestTypeFinder
	{
		Type FindRequestType(string requestName);
	}
}
