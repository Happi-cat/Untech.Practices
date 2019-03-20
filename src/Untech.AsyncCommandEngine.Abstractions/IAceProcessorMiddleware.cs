using System.Threading.Tasks;

namespace Untech.AsyncCommandEngine
{
	public interface IAceProcessorMiddleware
	{
		Task ExecuteAsync(AceContext context, AceRequestProcessorDelegate next);
	}
}