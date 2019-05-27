using System;
using System.Collections.Generic;
using System.Linq;
using AsyncCommandEngine.Run.Commands;
using Microsoft.Extensions.Logging;
using Untech.AsyncCommandEngine;
using Untech.AsyncCommandEngine.Processing;
using Untech.Practices.CQRS.Dispatching;

namespace AsyncCommandEngine.Run
{
	internal class CqrsStrategy : ICqrsStrategy, ITypeResolver
	{
		private readonly ILogger _logger;

		private readonly IReadOnlyList<Type> _types = new List<Type>
		{
			typeof(CompositeCommand), typeof(DelayCommand), typeof(ThrowCommand),typeof(HelloCommand)
		};

		public CqrsStrategy(ILogger logger)
		{
			_logger = logger;
		}

		public T ResolveOne<T>() where T : class
		{
			return new DemoHandlers(_logger) as T;
		}

		public IEnumerable<T> ResolveMany<T>() where T : class
		{
			return Enumerable.Empty<T>();
		}

		public Type FindRequestType(string requestName)
		{
			return _types.FirstOrDefault(t => t.FullName == requestName) ?? throw new ArgumentException($"There is no {requestName}");
		}

		public IDispatcher GetDispatcher(Context context)
		{
			return new Dispatcher(this);
		}
	}
}