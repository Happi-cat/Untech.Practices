using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Untech.AsyncJob.Builder.Registrars;
using Untech.AsyncJob.Metadata;

namespace Untech.AsyncJob.Builder.Factories
{
	internal class RequestMetadataProviderFactory : IServiceFactory<IRequestMetadataProvider>
	{
		private readonly MultiRegistrar<IRequestMetadataProvider> _registrar;

		public RequestMetadataProviderFactory(MultiRegistrar<IRequestMetadataProvider> registrar)
		{
			_registrar = registrar;
		}

		public IRequestMetadataProvider Create(IServiceProvider serviceProvider)
		{
			var internals = _registrar.CreateAll<IRequestMetadataProvider>(serviceProvider);
			if (internals.Count == 0) return NullRequestMetadataProvider.Instance;
			if (internals.Count == 1) return internals.First();
			return new CompositeRequestMetadataProvider(internals);
		}
	}
}