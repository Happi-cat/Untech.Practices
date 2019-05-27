using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Untech.AsyncCommandEngine.Metadata.Annotations;

namespace Untech.AsyncCommandEngine.Metadata
{
	/// <summary>
	/// Represents interface that provides methods for accessing attributes for the current <see cref="Request"/>.
	/// </summary>
	public interface IRequestMetadata
	{
		/// <summary>
		/// Gets single attribute of <typeparamref name="TAttr"/> type.
		/// </summary>
		/// <typeparam name="TAttr">The type of attribute to get.</typeparam>
		/// <returns>Instance of the <paramref name="TAttr"/> if found; otherwise null.
		/// May throw <see cref="InvalidOperationException"/> when multiple attributes were found.</returns>
		TAttr GetAttribute<TAttr>() where TAttr: MetadataAttribute;

		/// <summary>
		/// Gets all attributes of <typeparamref name="TAttr"/> type.
		/// </summary>
		/// <typeparam name="TAttr">The type of attribute to get.</typeparam>
		/// <returns>Collection of the <typeparamref name="TAttr"/> attributes.</returns>
		IEnumerable<TAttr> GetAttributes<TAttr>() where TAttr: MetadataAttribute;
	}
}