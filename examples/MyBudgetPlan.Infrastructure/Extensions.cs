using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyBudgetPlan.Domain;
using Untech.Practices.CQRS.Dispatching;

namespace MyBudgetPlan.Infrastructure
{
	internal static class Extensions
	{
		public static Task PublishAsync(this INotificationDispatcher dispatcher, BudgetLogEntry item, CancellationToken cancellationToken)
		{
			return Task.WhenAll(item.NotificationsToRaise.Select(n => dispatcher.PublishAsync(n, cancellationToken)));
		}
	}
}