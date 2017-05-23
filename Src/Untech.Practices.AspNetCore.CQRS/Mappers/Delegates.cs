using Microsoft.Extensions.Primitives;

namespace Untech.Practices.AspNetCore.CQRS.Mappers
{
	public delegate T Parser<out T>(StringValues input);
	public delegate string Formatter<in T>(T input);
}
