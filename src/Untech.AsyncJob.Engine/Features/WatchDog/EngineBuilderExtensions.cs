using System;
using Untech.AsyncJob.Builder;
using Untech.AsyncJob.Processing;

namespace Untech.AsyncJob.Features.WatchDog
{
	/// <summary>
	/// Extensions for watch-dog feature configuration.
	/// </summary>
	public static class EngineBuilderExtensions
	{
		/// <summary>
		/// Registers watch-dog middleware using <paramref name="builder"/>.
		/// </summary>
		/// <param name="builder">The builder to use for registration.</param>
		/// <returns><paramref name="builder"/></returns>
		/// <exception cref="ArgumentNullException">
		/// 	<paramref name="builder"/> is null.
		/// </exception>
		public static IRegistrar<IRequestProcessorMiddleware> AddWatchDog(this IRegistrar<IRequestProcessorMiddleware> builder)
		{
			return AddWatchDog(builder, _ => { });
		}

		/// <summary>
		/// Registers watch-dog middleware using <paramref name="builder"/>.
		/// </summary>
		/// <param name="builder">The builder to use for registration.</param>
		/// <param name="options">The watch-dog configuration.</param>
		/// <returns><paramref name="builder"/></returns>
		/// <exception cref="ArgumentNullException">
		/// 	<paramref name="builder"/> or <paramref name="options"/> is null.
		/// </exception>
		public static IRegistrar<IRequestProcessorMiddleware> AddWatchDog(this IRegistrar<IRequestProcessorMiddleware> builder, Action<WatchDogOptions> configureOptions)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));

			var options = OptionsBuilder.ConfigureAndValidate(configureOptions);

			return builder.Add(ctx => new WatchDogMiddleware(options, ctx.GetLogger()));
		}
	}
}
