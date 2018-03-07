namespace Untech.Practices.Mappers
{
	/// <summary>
	/// Extends <see cref="IMap{TIn,TOut}"/> and defines method for object mapping using already existing destination object.
	/// </summary>
	/// <typeparam name="TIn">The type of input object.</typeparam>
	/// <typeparam name="TOut">The type of destination object.</typeparam>
	public interface IExtendedMap<in TIn, TOut> : IMap<TIn, TOut>
	{
		/// <summary>
		/// Maps the specified input value into <paramref name="dest"/>.
		/// </summary>
		/// <param name="input">The input object to map.</param>
		/// <param name="dest">The destination object to use.</param>
		/// <returns>Populated <paramref name="dest"/>.</returns>
		TOut Map(TIn input, TOut dest);
	}
}