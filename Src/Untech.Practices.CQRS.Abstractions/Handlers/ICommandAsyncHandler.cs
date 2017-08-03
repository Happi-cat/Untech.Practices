﻿namespace Untech.Practices.CQRS.Handlers
{
	/// <summary>
	/// Defines an async handler for a command.
	/// </summary>
	/// <typeparam name="TIn">The type of command to be handled.</typeparam>
	/// <typeparam name="TOut">The type of result from the handler.</typeparam>
	public interface ICommandAsyncHandler<in TIn, TOut> : IRequestAsyncHandler<TIn, TOut>
		where TIn : ICommand<TOut>
	{
	}
}