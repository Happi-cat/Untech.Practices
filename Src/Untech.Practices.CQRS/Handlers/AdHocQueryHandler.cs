using System;

namespace Untech.Practices.CQRS.Handlers
{
	/// <summary>
	/// Represents adapter for <see cref="Func{TIn, TOut}"/>.
	/// </summary>
	/// <typeparam name="TIn">The type of command to be handled.</typeparam>
	/// <typeparam name="TOut">The type of result from the handler.</typeparam>
	public sealed class AdHocQueryHandler<TIn, TOut> : IQueryHandler<TIn, TOut>
		where TIn : IQuery<TOut>
	{
		private readonly Func<TIn, TOut> _func;

		public AdHocQueryHandler(Func<TIn, TOut> func)
		{
			Guard.CheckNotNull(nameof(func), func);

			_func = func;
		}

		public TOut Handle(TIn query) => _func(query);
	}
}