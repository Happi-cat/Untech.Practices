using System.Threading;
using System.Threading.Tasks;
using MyBudgetPlan.Domain.ExpenseLog.Forecast;
using Untech.Practices;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.DataStorage;

namespace MyBudgetPlan.Infrastructure
{
	public class ProjectedExpenseService : ICommandAsyncHandler<CreateProjectedExpense, ProjectedExpense>,
		ICommandAsyncHandler<UpdateProjectedExpense, ProjectedExpense>,
		ICommandAsyncHandler<DeleteProjectedExpense, Nothing>
	{
		private readonly IAsyncDataStorage<ProjectedExpense> _dataStorage;
		private readonly INotificationDispatcher _notificationDispatcher;

		public ProjectedExpenseService(IAsyncDataStorage<ProjectedExpense> dataStorage,
			INotificationDispatcher notificationDispatcher)
		{
			_dataStorage = dataStorage;
			_notificationDispatcher = notificationDispatcher;
		}

		public async Task<ProjectedExpense> HandleAsync(CreateProjectedExpense request, CancellationToken cancellationToken)
		{
			var item = new ProjectedExpense(request);

			item = await _dataStorage.CreateAsync(item, cancellationToken);
			await PublishNotifications(item);

			return item;
		}

		public async Task<ProjectedExpense> HandleAsync(UpdateProjectedExpense request, CancellationToken cancellationToken)
		{
			var item = await _dataStorage.FindAsync(request.Key, cancellationToken);

			item.Update(request);

			await _dataStorage.UpdateAsync(item, cancellationToken);
			await PublishNotifications(item);

			return item;
		}

		public async Task<Nothing> HandleAsync(DeleteProjectedExpense request, CancellationToken cancellationToken)
		{
			var item = await _dataStorage.FindAsync(request.Key, cancellationToken);

			await _dataStorage.DeleteAsync(item, cancellationToken);

			return Nothing.AtAll;
		}

		private Task PublishNotifications(ProjectedExpense item)
		{
			return _notificationDispatcher.PublishAsync(item, CancellationToken.None);
		}
	}
}