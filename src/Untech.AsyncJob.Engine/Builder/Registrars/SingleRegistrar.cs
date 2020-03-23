using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Untech.AsyncJob.Builder.Registrars
{
	internal class SingleRegistrar<T> : IRegistrar<T>, IEnumerable<ServiceDescriptor>
		where T : class
	{
		private readonly IServiceCollection _services = new ServiceCollection();

		public IRegistrar<T> Add<TImpl>() where TImpl : class, T
		{
			EnsureEmpty();
			_services.AddSingleton<T, TImpl>();
			return this;
		}

		public IRegistrar<T> Add(T instance)
		{
			EnsureEmpty();
			_services.AddSingleton<T>(instance);
			return this;
		}

		public IRegistrar<T> Add(Func<IServiceProvider, T> factory)
		{
			EnsureEmpty();
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

		private void EnsureEmpty()
		{
			if (_services.Count > 0) throw new InvalidOperationException("Cannot register more items of the specified type");
		}
	}
}