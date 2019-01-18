using System.Threading;
using System.Threading.Tasks;
using MyBudgetPlan.Domain.Forecasts;
using Untech.Practices;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.DataStorage;

namespace MyBudgetPlan.Infrastructure
{
	public class ForecastService : ICommandAsyncHandler<CreateForecast, Forecast>,
		ICommandAsyncHandler<UpdateForecast, Forecast>,
		ICommandAsyncHandler<DeleteForecast, Nothing>
	{
		private readonly IAsyncDataStorage<Forecast> _dataStorage;
		private readonly INotificationDispatcher _notificationDispatcher;

		public ForecastService(IAsyncDataStorage<Forecast> dataStorage,
			INotificationDispatcher notificationDispatcher)
		{
			_dataStorage = dataStorage;
			_notificationDispatcher = notificationDispatcher;
		}

		public async Task<Forecast> HandleAsync(CreateForecast request, CancellationToken cancellationToken)
		{
			var item = new Forecast(request);

			item = await _dataStorage.CreateAsync(item, cancellationToken);
			await PublishNotifications(item);

			return item;
		}

		public async Task<Forecast> HandleAsync(UpdateForecast request, CancellationToken cancellationToken)
		{
			var item = await _dataStorage.FindAsync(request.Key, cancellationToken);

			item.Update(request);

			await _dataStorage.UpdateAsync(item, cancellationToken);
			await PublishNotifications(item);

			return item;
		}

		public async Task<Nothing> HandleAsync(DeleteForecast request, CancellationToken cancellationToken)
		{
			var item = await _dataStorage.FindAsync(request.Key, cancellationToken);

			await _dataStorage.DeleteAsync(item, cancellationToken);

			return Nothing.AtAll;
		}

		private Task PublishNotifications(Forecast item)
		{
			return _notificationDispatcher.PublishAsync(item, CancellationToken.None);
		}
	}
}