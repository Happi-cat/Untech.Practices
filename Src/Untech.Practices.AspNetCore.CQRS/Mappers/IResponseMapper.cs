using Microsoft.AspNetCore.Http;

namespace Untech.Practices.AspNetCore.CQRS.Mappers
{
	public interface IResponseMapper<TIn>
	{
		void Map(TIn input, HttpResponse response);
	}
}
