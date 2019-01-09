using System.Threading;
using System.Threading.Tasks;
using MyBudgetPlan.Domain.IncomeLog.Actual;
using Untech.Practices;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.DataStorage;

namespace MyBudgetPlan.Infrastructure
{
	public class ActualIncomeService : ICommandAsyncHandler<CreateActualIncome, ActualIncome>,
		ICommandAsyncHandler<UpdateActualIncome, ActualIncome>,
		ICommandAsyncHandler<DeleteActualIncome, Nothing>
	{
		private readonly IAsyncDataStorage<ActualIncome> _dataStorage;
		private readonly INotificationDispatcher _notificationDispatcher;

		public ActualIncomeService(IAsyncDataStorage<ActualIncome> dataStorage,
			INotificationDispatcher notificationDispatcher)
		{
			_dataStorage = dataStorage;
			_notificationDispatcher = notificationDispatcher;
		}

		public async Task<ActualIncome> HandleAsync(CreateActualIncome request, CancellationToken cancellationToken)
		{
			var item = new ActualIncome(request);

			item = await _dataStorage.CreateAsync(item, cancellationToken);
			await PublishNotifications(item);

			return item;
		}

		public async Task<ActualIncome> HandleAsync(UpdateActualIncome request, CancellationToken cancellationToken)
		{
			var item = await _dataStorage.FindAsync(request.Key, cancellationToken);

			item.Update(request);

			await _dataStorage.UpdateAsync(item, cancellationToken);
			await PublishNotifications(item);

			return item;
		}

		public async Task<Nothing> HandleAsync(DeleteActualIncome request, CancellationToken cancellationToken)
		{
			var item = await _dataStorage.FindAsync(request.Key, cancellationToken);

			await _dataStorage.DeleteAsync(item, cancellationToken);

			return Nothing.AtAll;
		}

		private Task PublishNotifications(ActualIncome item)
		{
			return _notificationDispatcher.PublishAsync(item, CancellationToken.None);
		}
	}
}