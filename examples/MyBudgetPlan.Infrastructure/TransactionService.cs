using System.Threading;
using System.Threading.Tasks;
using MyBudgetPlan.Domain.Transactions;
using Untech.Practices;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.DataStorage;

namespace MyBudgetPlan.Infrastructure
{
	public class TransactionService : ICommandAsyncHandler<CreateTransaction, Transaction>,
		ICommandAsyncHandler<UpdateTransaction, Transaction>,
		ICommandAsyncHandler<DeleteTransaction, Nothing>
	{
		private readonly IAsyncDataStorage<Transaction> _dataStorage;
		private readonly INotificationDispatcher _notificationDispatcher;

		public TransactionService(IAsyncDataStorage<Transaction> dataStorage,
			INotificationDispatcher notificationDispatcher)
		{
			_dataStorage = dataStorage;
			_notificationDispatcher = notificationDispatcher;
		}

		public async Task<Transaction> HandleAsync(CreateTransaction request, CancellationToken cancellationToken)
		{
			var item = new Transaction(request);

			item = await _dataStorage.CreateAsync(item, cancellationToken);
			await PublishNotifications(item);

			return item;
		}

		public async Task<Transaction> HandleAsync(UpdateTransaction request, CancellationToken cancellationToken)
		{
			var item = await _dataStorage.FindAsync(request.Key, cancellationToken);

			item.Update(request);

			await _dataStorage.UpdateAsync(item, cancellationToken);
			await PublishNotifications(item);

			return item;
		}

		public async Task<Nothing> HandleAsync(DeleteTransaction request, CancellationToken cancellationToken)
		{
			var item = await _dataStorage.FindAsync(request.Key, cancellationToken);

			await _dataStorage.DeleteAsync(item, cancellationToken);

			return Nothing.AtAll;
		}

		private Task PublishNotifications(Transaction item)
		{
			return _notificationDispatcher.PublishAsync(item, CancellationToken.None);
		}
	}
}