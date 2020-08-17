using System.Threading;
using System.Threading.Tasks;
using MyBudgetPlan.Domain;
using MyBudgetPlan.Domain.Transactions;
using Untech.Practices.CQRS;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.Persistence;

namespace MyBudgetPlan.Infrastructure
{
	public class TransactionService : ICommandHandler<CreateTransaction, Transaction>,
		ICommandHandler<UpdateTransaction, Transaction>,
		ICommandHandler<DeleteTransaction, None>
	{
		private readonly IDataStorage<Transaction> _dataStorage;
		private readonly IEventDispatcher _eventDispatcher;

		public TransactionService(IDataStorage<Transaction> dataStorage,
			IEventDispatcher eventDispatcher)
		{
			_dataStorage = dataStorage;
			_eventDispatcher = eventDispatcher;
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
			var item = await _dataStorage.GetAsync(request.Key, cancellationToken);

			item.Update(request);

			await _dataStorage.UpdateAsync(item, cancellationToken);
			await PublishNotifications(item);

			return item;
		}

		public async Task<None> HandleAsync(DeleteTransaction request, CancellationToken cancellationToken)
		{
			var item = await _dataStorage.GetAsync(request.Key, cancellationToken);

			await _dataStorage.DeleteAsync(item, cancellationToken);

			return None.Value;
		}

		private Task PublishNotifications(BudgetLogEntry item)
		{
			return _eventDispatcher.PublishAsync(item, CancellationToken.None);
		}
	}
}
