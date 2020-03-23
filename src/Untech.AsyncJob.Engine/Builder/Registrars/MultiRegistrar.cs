using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Untech.AsyncJob.Builder.Registrars
{
	internal class MultiRegistrar<T> : IRegistrar<T>, IEnumerable<ServiceDescriptor>
		where T : class
	{
		private readonly IServiceCollection _services = new ServiceCollection();

		public IRegistrar<T> Add<TImpl>() where TImpl : class, T
		{
			_services.AddSingleton<T, TImpl>();
			return this;
		}

		public IRegistrar<T> Add(T instance)
		{
			_services.AddSingleton<T>(instance);
			return this;
		}

		public IRegistrar<T> Add(Func<IServiceProvider, T> factory)
		{
			_services.AddSingleton<T>(factory);
			return this;
		}

		public IEnumerator<ServiceDescriptor> GetEnumerator()
		{
			return _services.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}




}