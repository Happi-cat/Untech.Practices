using System;

namespace Untech.AsyncCommandEngine.Features.Throttling
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
		public static EngineBuilder ThenThrottling(this EngineBuilder builder)
		{
			return ThenThrottling(builder, new ThrottleOptions());
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
		public static EngineBuilder ThenThrottling(this EngineBuilder builder, ThrottleOptions options)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (options == null) throw new ArgumentNullException(nameof(options));

			builder.EnsureOptionsValid(options);

			return builder.Then(ctx => new ThrottleMiddleware(options));
		}
	}
}