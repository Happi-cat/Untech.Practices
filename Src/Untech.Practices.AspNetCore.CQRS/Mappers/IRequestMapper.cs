using Microsoft.AspNetCore.Http;

namespace Untech.Practices.AspNetCore.CQRS.Mappers
{
	public interface IRequestMapper<TOut>
	{
		TOut Map(HttpRequest request);
	}
}
