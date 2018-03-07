using System;

namespace Untech.Practices.Mappers
{
	public class AdHocMap<TIn, TOut> : IMap<TIn, TOut>
	{
		private readonly Func<TIn, TOut> _converter;

		public AdHocMap(Func<TIn, TOut> converter)
		{
			_converter = converter ?? throw new ArgumentNullException(nameof(converter));
		}

		public TOut Map(TIn input)
		{
			return _converter(input);
		}
	}
}
