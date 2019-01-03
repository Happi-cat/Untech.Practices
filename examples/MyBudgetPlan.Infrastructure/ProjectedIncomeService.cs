using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyBudgetPlan.Domain.IncomeLog.Forecast;
using Untech.Practices;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.DataStorage;

namespace MyBudgetPlan.Infrastructure
{
	public class ProjectedIncomeService : ICommandAsyncHandler<CreateProjectedIncome, ProjectedIncome>,
		ICommandAsyncHandler<UpdateProjectedIncome, ProjectedIncome>,
		ICommandAsyncHandler<DeleteProjectedIncome, Nothing>
	{
		private readonly IAsyncDataStorage<ProjectedIncome> _dataStorage;
		private readonly INotificationDispatcher _notificationDispatcher;

		public ProjectedIncomeService(IAsyncDataStorage<ProjectedIncome> dataStorage,
			INotificationDispatcher notificationDispatcher)
		{
			_dataStorage = dataStorage;
			_notificationDispatcher = notificationDispatcher;
		}

		public async Task<ProjectedIncome> HandleAsync(CreateProjectedIncome request, CancellationToken cancellationToken)
		{
			var item = new ProjectedIncome(request);

			await _dataStorage.CreateAsync(item, cancellationToken);
			await PublishNotifications(item);

			return item;
		}

		public async Task<ProjectedIncome> HandleAsync(UpdateProjectedIncome request, CancellationToken cancellationToken)
		{
			var item = await _dataStorage.FindAsync(request.Key, cancellationToken);

			item.Update(request);

			await _dataStorage.UpdateAsync(item, cancellationToken);
			await PublishNotifications(item);

			return item;
		}

		public async Task<Nothing> HandleAsync(DeleteProjectedIncome request, CancellationToken cancellationToken)
		{
			var item = await _dataStorage.FindAsync(request.Key, cancellationToken);

			await _dataStorage.DeleteAsync(item, cancellationToken);

			return Nothing.AtAll;
		}

		private Task PublishNotifications(ProjectedIncome item)
		{
			return _notificationDispatcher.PublishAsync(item, CancellationToken.None);
		}
	}
}