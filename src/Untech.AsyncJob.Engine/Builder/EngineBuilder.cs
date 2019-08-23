using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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

		private readonly Action<OrchestratorOptions> _configureOptions;

		public EngineBuilder()
			: this(_ => { })
		{

		}

		public EngineBuilder(Action<OrchestratorOptions> configureOptions)
		{
			_container = new ServiceContainer(s_defaultServices);
			_configureOptions = configureOptions;
		}

		public EngineBuilder(IServiceProvider provider, Action<OrchestratorOptions> configureOptions)
		{
			_container = new ServiceContainer(new CompositeServiceProvider(new [] { provider, s_defaultServices }));
			_configureOptions = configureOptions;
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
		/// <param name="loggerCreator">The logger factory.</param>
		/// <returns></returns>
		public IEngineBuilder LogTo(Func<ILoggerFactory> loggerCreator)
		{
			AddService(loggerCreator);
			return this;
		}

		/// <summary>
		/// Sets <see cref="ITransport"/> that will be used as a request store.
		/// </summary>
		/// <param name="transportCreator">The transport to use.</param>
		/// <returns></returns>
		public IEngineBuilder ReceiveRequestsFrom(Func<IServiceProvider, ITransport> transportCreator)
		{
			AddService(transportCreator);
			return this;
		}

		/// <summary>
		/// Sets <see cref="IRequestMetadataProvider"/> that can be used for getting <see cref="IRequestMetadata"/>.
		/// </summary>
		/// <param name="providerCreator">The provider to use.</param>
		/// <returns></returns>
		public IEngineBuilder ReadMetadataFrom(Func<IServiceProvider, IRequestMetadataProvider> providerCreator)
		{
			AddService(providerCreator);
			return this;
		}

		public IEngineBuilder Do(Action<PipelineBuilder> configureProcessor)
		{
			var pipeline = new PipelineBuilder();
			configureProcessor(pipeline);

			AddServices(provider => pipeline.Select(b => b(provider)).ToList());

			return this;
		}

		/// <summary>
		/// Returns constructed instance of the <see cref="IRequestProcessor"/>.
		/// </summary>
		/// <returns></returns>
		public IRequestProcessor BuildProcessor()
		{
			return new RequestProcessor(
				_container.GetService<IEnumerable<IRequestProcessorMiddleware>>()
			);
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
				BuildProcessor(),
				_container.GetLogger().CreateLogger<Orchestrator>()
			);
		}

		private void AddService<T>(Func<T> creator)
		{
			_container.AddService(typeof(T), (provider, type) => creator());
		}

		private void AddService<T>(Func<IServiceProvider, T> creator)
		{
			_container.AddService(typeof(T), (provider, type) => creator(provider));
		}

		private void AddServices<T>(Func<IServiceProvider, IEnumerable<T>> creator)
		{
			AddService(creator);
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
