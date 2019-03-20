using System.Threading.Tasks;

namespace Untech.AsyncCommmandEngine.Abstractions
{
	public interface IAceProcessorMiddleware
	{
		Task Execute(AceContext context, AceRequestProcessorDelegate next);
	}
}