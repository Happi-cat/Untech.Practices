using System;

namespace Untech.AsyncCommandEngine.Features.WatchDog
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
		public static MiddlewareCollection ThenWatchDog(this MiddlewareCollection builder)
		{
			return ThenWatchDog(builder, _ => {});
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
		public static MiddlewareCollection ThenWatchDog(this MiddlewareCollection builder, Action<WatchDogOptions> configureOptions)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (configureOptions == null) throw new ArgumentNullException(nameof(configureOptions));

			var options = new WatchDogOptions();
			configureOptions(options);
			//builder.EnsureOptionsValid(options);

			return builder.Then(ctx => new WatchDogMiddleware(options, ctx.GetLogger()));
		}
	}
}