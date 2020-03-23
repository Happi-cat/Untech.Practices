using System;
using Untech.AsyncJob.Processing;

namespace Untech.AsyncJob.Builder
{
	public interface IPipelineBuilder
	{

		/// <summary>
		/// Returns constructed instance of the <see cref="IRequestProcessor"/>.
		/// </summary>
		/// <returns></returns>
		IRequestProcessor BuildProcessor();
	}
}