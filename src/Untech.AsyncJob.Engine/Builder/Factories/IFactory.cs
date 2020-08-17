using System;

namespace Untech.AsyncJob.Builder.Factories
{
	internal interface IServiceFactory<T> where T : class
	{
		T Create(IServiceProvider serviceProvider);
	}
}