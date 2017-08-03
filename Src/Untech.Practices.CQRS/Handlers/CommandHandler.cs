﻿namespace Untech.Practices.CQRS.Handlers
{
	/// <summary>
	/// Represents base command handler without a response.
	/// </summary>
	/// <typeparam name="TIn">The type of command to be handled.</typeparam>
	public abstract class CommandHandler<TIn> : ICommandHandler<TIn, Unit>
		where TIn : ICommand<Unit>
	{
		public Unit Handle(TIn request)
		{
			HandleCore(request);

			return Unit.Value;
		}

		protected abstract void HandleCore(TIn command);
	}
}