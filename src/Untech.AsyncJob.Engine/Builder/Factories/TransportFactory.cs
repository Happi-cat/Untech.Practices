using System;
using System.Linq;
using Untech.AsyncJob.Builder.Registrars;
using Untech.AsyncJob.Transports;

namespace Untech.AsyncJob.Builder.Factories
{
	internal class TransportFactory : IServiceFactory<ITransport>
	{
		private readonly MultiRegistrar<ITransport> _registrar;

		public TransportFactory(MultiRegistrar<ITransport> registrar)
		{
			_registrar = registrar;
		}

		public ITransport Create(IServiceProvider serviceProvider)
		{
			var internals = _registrar.CreateAll<ITransport>(serviceProvider);
			if (internals.Count == 0) throw new InvalidOperationException("No registered transports");
			if (internals.Count == 1) return internals.First();
			return new CompositeTransport(internals);
		}
	}
}