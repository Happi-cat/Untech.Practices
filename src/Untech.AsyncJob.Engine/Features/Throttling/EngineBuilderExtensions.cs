using System;
using Untech.AsyncJob.Builder;
using Untech.AsyncJob.Processing;

namespace Untech.AsyncJob.Features.Throttling
{
	/// <summary>
	/// Extensions for throttling feature configuration.
	/// </summary>
	public static class EngineBuilderExtensions
	{
		/// <summary>
		/// Registers throttling middleware using <paramref name="builder"/>.
		/// </summary>
		/// <param name="builder">The builder to use for registration.</param>
		/// <returns><paramref name="builder"/></returns>
		/// <exception cref="ArgumentNullException">
		/// 	<paramref name="builder"/> is null.
		/// </exception>
		public static IRegistrar<IRequestProcessorMiddleware> AddThrottling(this IRegistrar<IRequestProcessorMiddleware> builder)
		{
			return AddThrottling(builder, _ => { });
		}

		/// <summary>
		/// Registers throttling middleware using <paramref name="builder"/> with the specified <paramref name="options"/>.
		/// </summary>
		/// <param name="builder">The builder to use for registration.</param>
		/// <param name="options">The throttling configuration.</param>
		/// <returns><paramref name="builder"/></returns>
		/// <exception cref="ArgumentNullException">
		/// 	<paramref name="builder"/> or <paramref name="options"/> is null.
		/// </exception>
		public static IRegistrar<IRequestProcessorMiddleware> AddThrottling(this IRegistrar<IRequestProcessorMiddleware> builder, Action<ThrottleOptions> configureOptions)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (configureOptions == null) throw new ArgumentNullException(nameof(configureOptions));

			var options = OptionsBuilder.ConfigureAndValidate(configureOptions);

			return builder.Add(ctx => new ThrottleMiddleware(options));
		}
	}
}
