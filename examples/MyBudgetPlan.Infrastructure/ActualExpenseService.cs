using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyBudgetPlan.Domain.ExpenseLog.Actual;
using Untech.Practices;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.DataStorage;

namespace MyBudgetPlan.Infrastructure
{
	public class ActualExpenseService : ICommandAsyncHandler<CreateActualExpense, ActualExpense>,
		ICommandAsyncHandler<UpdateActualExpense, ActualExpense>,
		ICommandAsyncHandler<DeleteActualExpense, Nothing>
	{
		private readonly IAsyncDataStorage<ActualExpense> _dataStorage;
		private readonly INotificationDispatcher _notificationDispatcher;

		public ActualExpenseService(IAsyncDataStorage<ActualExpense> dataStorage,
			INotificationDispatcher notificationDispatcher)
		{
			_dataStorage = dataStorage;
			_notificationDispatcher = notificationDispatcher;
		}

		public async Task<ActualExpense> HandleAsync(CreateActualExpense request, CancellationToken cancellationToken)
		{
			var item = new ActualExpense(request);

			await _dataStorage.CreateAsync(item, cancellationToken);
			await PublishNotifications(item);

			return item;
		}

		public async Task<ActualExpense> HandleAsync(UpdateActualExpense request, CancellationToken cancellationToken)
		{
			var item = await _dataStorage.FindAsync(request.Key, cancellationToken);

			item.Update(request);

			await _dataStorage.UpdateAsync(item, cancellationToken);
			await PublishNotifications(item);

			return item;
		}

		public async Task<Nothing> HandleAsync(DeleteActualExpense request, CancellationToken cancellationToken)
		{
			var item = await _dataStorage.FindAsync(request.Key, cancellationToken);

			await _dataStorage.DeleteAsync(item, cancellationToken);

			return Nothing.AtAll;
		}

		private Task PublishNotifications(ActualExpense item)
		{
			return _notificationDispatcher.PublishAsync(item, CancellationToken.None);
		}
	}
}