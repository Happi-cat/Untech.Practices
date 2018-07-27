using System.Collections.Generic;

namespace Untech.Practices.CQRS.Dispatching
{
	/// <summary>
	///     Represents interface that can resolve CQRS handlers.
	/// </summary>
	public interface ITypeResolver
	{
		/// <summary>
		///     Gets instantiated object of type <typeparamref name="T" />.
		/// </summary>
		/// <typeparam name="T">The type to be resolved.</typeparam>
		/// <returns>Instantiated object.</returns>
		T ResolveOne<T>() where T : class;

		/// <summary>
		///     Gets all possible instantiated objects of type <typeparamref name="T" />.
		///     Can be used when <typeparamref name="T" /> is a base class or interface.
		/// </summary>
		/// <typeparam name="T">The type to be resolved.</typeparam>
		/// <returns>The collection of instantiated objects.</returns>
		IEnumerable<T> ResolveMany<T>() where T : class;
	}
}