using System;

namespace Untech.AsyncJob.Builder
{
	public interface IRegistrar<T> where T : class
	{
		IRegistrar<T> Add<TImpl>() where TImpl : class, T;

		IRegistrar<T> Add(T instance);

		IRegistrar<T> Add(Func<IServiceProvider, T> factory);
	}
}