using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;
using Untech.Practices.CQRS;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.DataStorage;
using Untech.Practices.DataStorage.Linq2Db;

namespace Untech.AsyncJob.Transports.Sql
{
	public class SqlRequest : Request, IHasKey<string>
	{
		private SqlRequest() {}

		public SqlRequest(object requestPayload, QueueOptions options = null)
		{
			Identifier = Guid.NewGuid().ToString("B");
			Name = requestPayload.GetType().FullName;
			Created = DateTimeOffset.Now;

			Attributes = options?.Advanced?.ToDictionary(n => n.Key, n => Convert.ToString(n.Value));
			Priority = options?.Priority ?? 0;
			ExecuteAfter = options?.ExecuteAfter != null ? Created + options.ExecuteAfter : null;
			ExpiresAfter = options?.ExpiresAfter != null ? Created + options.ExpiresAfter : null;

			Body = JsonSerializer.ToString(requestPayload);
		}

		public override string Identifier { get; }
		public override string Name { get; }
		public override DateTimeOffset Created { get; }

		public override IDictionary<string, string> Attributes { get; }

		public int Priority { get; set; }

		public DateTimeOffset? ExpiresAfter { get; set; }

		public DateTimeOffset? ExecuteAfter { get; set; }

		public string Body { get; set; }

		public override object GetBody(Type requestType)
		{
			return JsonSerializer.Parse(Body, requestType);
		}

		public override Stream GetRawBody()
		{
			throw new NotImplementedException();
		}

		public string Key => Identifier;
	}

	public class SqlTransport : GenericDataStorage<SqlRequest, string>, ITransport, IQueueDispatcher
	{
		public SqlTransport(Func<IDataContext> contextFactory) : base(contextFactory)
		{
		}

		public async Task<ReadOnlyCollection<Request>> GetRequestsAsync(int count)
		{
			using (var context = GetContext())
			{
				Table(context)
			}
			throw new NotImplementedException();
		}

		public async Task CompleteRequestAsync(Request request)
		{
			throw new NotImplementedException();
		}

		public async Task FailRequestAsync(Request request, Exception exception)
		{
			throw new NotImplementedException();
		}

		public async Task EnqueueAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default,
			QueueOptions options = null)
		{
			throw new NotImplementedException();
		}

		public async Task EnqueueAsync(INotification notification, CancellationToken cancellationToken = default,
			QueueOptions options = null)
		{
			throw new NotImplementedException();
		}


	}
}