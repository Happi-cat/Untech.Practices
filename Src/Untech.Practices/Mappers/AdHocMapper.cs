using System;

namespace Untech.Practices.Mappers
{
	public class AdHocMapper<TIn, TOut> : IMapper<TIn, TOut>
	{
		private readonly Func<TIn, TOut> _converter;

		public AdHocMapper(Func<TIn, TOut> converter)
		{
			Guard.CheckNotNull(nameof(converter), converter);

			_converter = converter;
		}

		public TOut Map(TIn input)
		{
			return _converter(input);
		}
	}
}
