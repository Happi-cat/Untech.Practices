namespace Untech.Practices.Mappers
{
	public interface IMapper<in TIn, out TOut>
	{
		/// <summary>
		/// Maps the specified input value.
		/// </summary>
		/// <param name="input">The input value to map.</param>
		/// <returns>The result value.</returns>
		TOut Map(TIn input);
	}
}
