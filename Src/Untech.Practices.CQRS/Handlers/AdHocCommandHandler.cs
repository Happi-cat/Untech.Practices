using System;

namespace Untech.Practices.CQRS.Handlers
{
	/// <summary>
	/// Represents adapter for <see cref="Func{TIn, TOut}"/>.
	/// </summary>
	/// <typeparam name="TIn">The type of command to be handled.</typeparam>
	/// <typeparam name="TOut">The type of result from the handler.</typeparam>
	public sealed class AdHocCommandHandler<TIn, TOut> : ICommandHandler<TIn, TOut>
		where TIn : ICommand<TOut>
	{
		private readonly Func<TIn, TOut> _func;

		public AdHocCommandHandler(Func<TIn, TOut> func)
		{
			_func = func ?? throw new ArgumentNullException(nameof(func));
		}

		public TOut Handle(TIn request) => _func(request);
	}
}