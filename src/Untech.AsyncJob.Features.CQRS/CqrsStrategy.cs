using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Untech.AsyncJob.Formatting;
using Untech.Practices.CQRS.Dispatching;

namespace Untech.AsyncJob.Features.CQRS
{
	public class CqrsStrategy : ICqrsStrategy
	{
		private readonly IRequestTypeFinder _requestTypeFinder;
		public CqrsStrategy(IRequestTypeFinder requestTypeFinder)
		{
			_requestTypeFinder = requestTypeFinder ?? throw new ArgumentNullException(nameof(requestTypeFinder));
		}

		[CanBeNull]
		public IDispatcher Dispatcher { get; set; }

		[CanBeNull]
		public IDispatcher FallbackDispatcher { get; set; }

		[CanBeNull]
		public IRequestContentFormatter Formatter { get; set; }

		[CanBeNull]
		public IRequestContentFormatter FallbackFormatter { get; set; }

		public IRequestTypeFinder GetRequestTypeFinder()
		{
			return _requestTypeFinder;
		}

		public IDispatcher GetDispatcher(Context context)
		{
			return Dispatcher ?? ResolveService(context, FallbackDispatcher);
		}

		public IRequestContentFormatter GetRequestFormatter(Context context)
		{
			return Formatter ?? ResolveService(context, FallbackFormatter);
		}

		private static T ResolveService<T>(Context context, T fallback)
		{
			return GetCandidates().FirstOrDefault(n => n != null);

			IEnumerable<T> GetCandidates()
			{
				if (TryGetItem<T>(context, out var service)) yield return service;

				if (TryGetItem<IServiceProvider>(context, out var serviceProvider))
					yield return TryGetService<T>(serviceProvider);

				yield return fallback;

				throw new InvalidOperationException($"Unable to resolve service {typeof(T)}");
			}
		}

		private static T TryGetService<T>(IServiceProvider serviceProvider)
		{
			try { return (T)serviceProvider.GetService(typeof(T)); }
			catch { return default(T); }
		}

		private static bool TryGetItem<T>(Context context, out T value)
		{
			if (context.Items.TryGetValue(typeof(T), out var obj))
			{
				value = (T)obj;
				return true;
			}

			value = default(T);
			return false;
		}
	}
}