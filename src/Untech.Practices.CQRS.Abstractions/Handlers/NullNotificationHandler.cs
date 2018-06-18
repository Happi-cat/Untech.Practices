﻿using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.CQRS.Handlers
{
	/// <summary>
	/// Represents dummy notification handler.
	/// </summary>
	/// <typeparam name="TIn">The type of notification to be handled.</typeparam>
	public sealed class NullNotificationHandler<TIn> :
		INotificationHandler<TIn>, INotificationAsyncHandler<TIn>
		where TIn : INotification
	{
		/// <inheritdoc />
		public void Publish(TIn notification)
		{
		}

		/// <inheritdoc />
		public Task PublishAsync(TIn notification, CancellationToken cancellationToken) => Task.FromResult(0);
	}
}