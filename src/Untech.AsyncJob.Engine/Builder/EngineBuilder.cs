using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Untech.AsyncJob.Builder.Factories;
using Untech.AsyncJob.Builder.Registrars;
using Untech.AsyncJob.Metadata;
using Untech.AsyncJob.Processing;
using Untech.AsyncJob.Transports;

namespace Untech.AsyncJob.Builder
{
	/// <summary>
	/// Used for <see cref="IOrchestrator"/> and <see cref="IRequestProcessor"/> configuration.
	/// </summary>
	public class EngineBuilder : IEngineBuilder
	{
		private static readonly IServiceProvider s_defaultServices;
		private readonly IServiceContainer _container;

		private readonly MultiRegistrar<ITransport> _transport;
		private readonly MultiRegistrar<IRequestMetadataProvider> _requestMetadataProvider;
		private readonly MultiRegistrar<IRequestProcessorMiddleware> _requestProcessorMiddleware;
		private readonly SingleRegistrar<IRequestProcessor> _requestProcessor;

		private readonly Action<OrchestratorOptions> _configureOptions;

		public EngineBuilder()
			: this(_ => { })
		{

		}

		public EngineBuilder(Action<OrchestratorOptions> configureOptions)
			: this(new ServiceContainer(s_defaultServices), configureOptions)
		{
		}

		public EngineBuilder(IServiceProvider provider, Action<OrchestratorOptions> configureOptions)
			: this(new ServiceContainer(new CompositeServiceProvider(new[] { provider, s_defaultServices })),
				configureOptions)
		{
		}

		private EngineBuilder(IServiceContainer container, Action<OrchestratorOptions> configureOptions)
		{
			_container = container;
			_configureOptions = configureOptions;

			_transport = new MultiRegistrar<ITransport>();
			_requestMetadataProvider = new MultiRegistrar<IRequestMetadataProvider>();
			_requestProcessorMiddleware = new MultiRegistrar<IRequestProcessorMiddleware>();
			_requestProcessor = new SingleRegistrar<IRequestProcessor>();

			RegisterFactory(_container, new TransportFactory(_transport));
			RegisterFactory(_container, new RequestMetadataProviderFactory(_requestMetadataProvider));
			RegisterFactory(_container, new RequestProcessorFactory(_requestProcessorMiddleware, _requestProcessor));
		}

		static EngineBuilder()
		{
			var container = new ServiceContainer();
			container.AddService(typeof(ILoggerFactory), NullLoggerFactory.Instance);
			container.AddService(typeof(IRequestMetadataProvider), NullRequestMetadataProvider.Instance);
			s_defaultServices = container;
		}

		/// <summary>
		/// Sets <see cref="ILoggerFactory"/> that will be used for logging.
		/// </summary>
		/// <param name="factory">The logger factory.</param>
		/// <returns></returns>
		public IEngineBuilder LogTo(Func<ILoggerFactory> factory)
		{
			_container.AddService(typeof(ILoggerFactory), (container, t) => factory());
			return this;
		}

		/// <summary>
		/// Sets <see cref="ITransport"/> that will be used as a request store.
		/// </summary>
		/// <param name="configure">The transport to use.</param>
		/// <returns></returns>
		public IEngineBuilder ReceiveRequestsFrom(Action<IRegistrar<ITransport>> configure)
		{
			configure(_transport);
			return this;
		}

		/// <summary>
		/// Sets <see cref="IRequestMetadataProvider"/> that can be used for getting <see cref="IRequestMetadata"/>.
		/// </summary>
		/// <param name="configure">The provider to use.</param>
		/// <returns></returns>
		public IEngineBuilder ReadMetadataFrom(Action<IRegistrar<IRequestMetadataProvider>> configure)
		{
			configure(_requestMetadataProvider);
			return this;
		}

		public IEngineBuilder Then(Action<IRegistrar<IRequestProcessorMiddleware>> configure)
		{
			configure(_requestProcessorMiddleware);
			return this;
		}

		public IEngineBuilder Finally(Action<IRegistrar<IRequestProcessor>> configure)
		{
			configure(_requestProcessor);
			return this;
		}

		public IRequestProcessor BuildProcessor()
		{
			return _container.GetService<IRequestProcessor>();
		}

		/// <summary>
		/// Returns constructed instance of the <see cref="IOrchestrator"/>.
		/// </summary>
		/// <param name="configureOptions">The action for orchestrator options configuration.</param>
		/// <returns></returns>
		public IOrchestrator BuildOrchestrator()
		{

			return new Orchestrator(
				OptionsBuilder.ConfigureAndValidate(_configureOptions),
				_container.GetService<ITransport>(),
				_container.GetService<IRequestMetadataProvider>(),
				_container.GetService<IRequestProcessor>(),
				_container.GetLogger().CreateLogger<Orchestrator>()
			);
		}

		private static void RegisterFactory<T>(IServiceContainer container, IServiceFactory<T> factory)
			where T : class
		{
			container.AddService(typeof(T), (provider, type) => factory.Create(provider));
		}

		private class CompositeServiceProvider : IServiceProvider
		{
			private readonly IEnumerable<IServiceProvider> _providers;

			public CompositeServiceProvider(IEnumerable<IServiceProvider> providers)
			{
				_providers = providers.ToList();
			}

			public object GetService(Type serviceType)
			{
				return _providers
					.Select(p => p.GetService(serviceType))
					.FirstOrDefault(s => s != null);
			}
		}
	}
}
