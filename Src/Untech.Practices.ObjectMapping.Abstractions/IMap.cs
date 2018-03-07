namespace Untech.Practices.ObjectMapping
{
	/// <summary>
	/// Defines method for object mapping from <typeparamref name="TIn"/> to <typeparamref name="TOut"/>.
	/// </summary>
	/// <typeparam name="TIn">The type of input object.</typeparam>
	/// <typeparam name="TOut">The type of destination object.</typeparam>
	public interface IMap<in TIn, out TOut>
	{
		/// <summary>
		/// Maps the specified input value.
		/// </summary>
		/// <param name="input">The input object to map.</param>
		/// <returns>The result value.</returns>
		TOut Map(TIn input);
	}
}
