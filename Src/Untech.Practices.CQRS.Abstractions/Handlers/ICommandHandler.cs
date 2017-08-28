﻿namespace Untech.Practices.CQRS.Handlers
{
	/// <summary>
	/// Defines a handler for a command.
	/// </summary>
	/// <typeparam name="TIn">The type of command to be handled.</typeparam>
	/// <typeparam name="TOut">The type of result from the handler.</typeparam>
	public interface ICommandHandler<in TIn, out TOut> : IRequestHandler<TIn, TOut>
		where TIn : ICommand<TOut>
	{
	}
}