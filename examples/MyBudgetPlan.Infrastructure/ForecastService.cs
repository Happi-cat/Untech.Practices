using System.Threading;
using System.Threading.Tasks;
using MyBudgetPlan.Domain;
using MyBudgetPlan.Domain.Forecasts;
using Untech.Practices;
using Untech.Practices.CQRS;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.DataStorage;

namespace MyBudgetPlan.Infrastructure
{
	public class ForecastService : ICommandHandler<CreateForecast, Forecast>,
		ICommandHandler<UpdateForecast, Forecast>,
		ICommandHandler<DeleteForecast, None>
	{
		private readonly IDataStorage<Forecast> _dataStorage;
		private readonly IEventDispatcher _eventDispatcher;

		public ForecastService(IDataStorage<Forecast> dataStorage,
			IEventDispatcher eventDispatcher)
		{
			_dataStorage = dataStorage;
			_eventDispatcher = eventDispatcher;
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
			var item = await _dataStorage.GetAsync(request.Key, cancellationToken);

			item.Update(request);

			await _dataStorage.UpdateAsync(item, cancellationToken);
			await PublishNotifications(item);

			return item;
		}

		public async Task<None> HandleAsync(DeleteForecast request, CancellationToken cancellationToken)
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