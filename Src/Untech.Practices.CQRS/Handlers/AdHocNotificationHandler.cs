using System;

namespace Untech.Practices.CQRS.Handlers
{
	/// <summary>
	/// Represents adapter for <see cref="Action{TIn}"/>.
	/// </summary>
	/// <typeparam name="TIn">The type of command to be handled.</typeparam>
	public sealed class AdHocNotificationHandler<TIn> : INotificationHandler<TIn>
		where TIn : INotification
	{
		private readonly Action<TIn> _func;

		public AdHocNotificationHandler(Action<TIn> func)
		{
			Guard.CheckNotNull(nameof(func), func);

			_func = func;
		}

		public void Publish(TIn notification) => _func(notification);
	}
}