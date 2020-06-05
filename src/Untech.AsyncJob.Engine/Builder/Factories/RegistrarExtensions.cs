using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Untech.AsyncJob.Builder.Factories
{
	internal static class RegistrarExtensions
	{
		public static T CreateOne<T>(this IEnumerable<ServiceDescriptor> services, IServiceProvider serviceProvider)
		{
			return services
				.Where(n => n.ServiceType == typeof(T))
				.Select(n => Create(n, serviceProvider))
				.OfType<T>()
				.FirstOrDefault();
		}

		public static IReadOnlyCollection<T> CreateAll<T>(this IEnumerable<ServiceDescriptor> services,
			IServiceProvider serviceProvider)
		{
			return services
				.Where(n => n.ServiceType == typeof(T))
				.Select(n => Create(n, serviceProvider))
				.OfType<T>()
				.ToArray();
		}

		private static object Create(ServiceDescriptor descriptor, IServiceProvider serviceProvider)
		{
			if (descriptor.ImplementationInstance != null) return descriptor.ImplementationInstance;
			if (descriptor.ImplementationFactory != null) return descriptor.ImplementationFactory(serviceProvider);
			if (descriptor.ImplementationType != null) return serviceProvider.GetService(descriptor.ImplementationType);

			throw new NotSupportedException();
		}
	}
}