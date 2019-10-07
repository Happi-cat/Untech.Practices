using System;
using Microsoft.Extensions.Logging;

namespace Untech.AsyncJob.Builder
{
	/// <summary>
	/// Contains set of methods that can be used during work with <see cref="IServiceProvider"/>.
	/// </summary>
	public static class ServiceProviderExtensions
	{
		public static T GetService<T>(this IServiceProvider provider)
		{
			return (T)provider.GetService(typeof(T));
		}

		public static ILoggerFactory GetLogger(this IServiceProvider provider)
		{
			return provider.GetService<ILoggerFactory>();
		}

		/// <summary>
		/// Gets instance of the <see cref="ILogger{TCategoryName}"/>
		/// </summary>
		/// <param name="provider"></param>
		/// <typeparam name="T">The type to use as category.</typeparam>
		/// <returns></returns>
		public static ILogger<T> GetLogger<T>(this IServiceProvider provider)
		{
			return provider.GetLogger().CreateLogger<T>();
		}

		/// <summary>
		/// Gets instance of the <see cref="ILogger"/> for the specified <paramref name="categoryName"/>.
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="categoryName">The name of category.</param>
		/// <returns></returns>
		public static ILogger GetLogger(this IServiceProvider provider, string categoryName)
		{
			return provider.GetLogger().CreateLogger(categoryName);
		}

	}
}