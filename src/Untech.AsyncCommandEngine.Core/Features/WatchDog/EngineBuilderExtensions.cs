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
		public static EngineBuilder ThenWatchDog(this EngineBuilder builder)
		{
			return ThenWatchDog(builder, new WatchDogOptions());
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
		public static EngineBuilder ThenWatchDog(this EngineBuilder builder, WatchDogOptions options)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (options == null) throw new ArgumentNullException(nameof(options));

			builder.EnsureOptionsValid(options);

			return builder.Then(ctx => new WatchDogMiddleware(options, ctx.GetLogger()));
		}
	}
}