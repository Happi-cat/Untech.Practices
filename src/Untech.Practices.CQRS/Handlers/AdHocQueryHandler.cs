using System;

namespace Untech.Practices.CQRS.Handlers
{
	/// <summary>
	///     Represents adapter for <see cref="Func{T,TResult}" />.
	/// </summary>
	/// <typeparam name="TIn">The type of command to be handled.</typeparam>
	/// <typeparam name="TOut">The type of result from the handler.</typeparam>
	public sealed class AdHocQueryHandler<TIn, TOut> : IQueryHandler<TIn, TOut>
		where TIn : IQuery<TOut>
	{
		private readonly Func<TIn, TOut> _func;

		public AdHocQueryHandler(Func<TIn, TOut> func)
		{
			_func = func ?? throw new ArgumentNullException(nameof(func));
		}

		public TOut Handle(TIn query)
		{
			return _func(query);
		}
	}
}