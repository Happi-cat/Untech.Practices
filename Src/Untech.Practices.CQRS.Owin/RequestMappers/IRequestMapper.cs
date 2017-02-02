using Microsoft.Owin;
using Untech.Practices.Mappers;

namespace Untech.Practices.CQRS.Owin.RequestMappers
{
	public interface IRequestMapper<out T> : IMapper<IOwinRequest, T>
	{
	}
}