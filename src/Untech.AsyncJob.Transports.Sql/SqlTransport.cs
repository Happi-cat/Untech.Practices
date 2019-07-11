using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;
using Untech.Practices.CQRS;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.DataStorage;
using Untech.Practices.DataStorage.Linq2Db;

namespace Untech.AsyncJob.Transports.Sql
{
	public class SqlTransport : ITransport, IQueueDispatcher
	{
		private readonly Func<IDataContext> _contextFactory;
		private readonly IDataStorage<RequestEntry, string> _requests;
		private readonly IDataStorage<RequestAuditEntry, int> _audit;

		public SqlTransport(Func<IDataContext> contextFactory)
		{
			_contextFactory = contextFactory;
			_requests = new GenericDataStorage<RequestEntry, string>(contextFactory);
			_audit = new GenericDataStorage<RequestAuditEntry, int>(contextFactory);
		}

		public async Task<ReadOnlyCollection<Request>> GetRequestsAsync(int count)
		{
			var now = DateTimeOffset.UtcNow;

			using (var context = _contextFactory())
			{
				var request = await context.GetTable<RequestEntry>()
					.Where(r => r.ExecuteAfter == null || r.ExecuteAfter < now)
					.Where(r => r.ExpiresAfter == null || now <= r.ExpiresAfter)
					.OrderByDescending(r => r.Priority)
					.Take(count)
					.ToListAsync();

				return request.Select(r => new SqlRequest(r)).ToList<Request>().AsReadOnly();
			}
		}

		public async Task CompleteRequestAsync(Request request)
		{
			var requestEntry = (RequestEntry)request.Items[typeof(RequestEntry)];

			await _audit.CreateAsync(new RequestAuditEntry(requestEntry)
			{
				Executed = DateTimeOffset.UtcNow, Succeeded = true
			});
			await _requests.DeleteAsync(requestEntry);
		}

		public async Task FailRequestAsync(Request request, Exception exception)
		{
			var requestEntry = (RequestEntry)request.Items[typeof(RequestEntry)];

			await _audit.CreateAsync(new RequestAuditEntry(requestEntry)
			{
				Executed = DateTimeOffset.UtcNow, Succeeded = false, Error = exception.ToString()
			});
			await _requests.DeleteAsync(requestEntry);
		}

		public Task EnqueueAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default,
			QueueOptions options = null)
		{
			return _requests.CreateAsync(new RequestEntry(command, options), cancellationToken);
		}

		public Task EnqueueAsync(INotification notification, CancellationToken cancellationToken = default,
			QueueOptions options = null)
		{
			return _requests.CreateAsync(new RequestEntry(notification, options), cancellationToken);
		}
	}
}