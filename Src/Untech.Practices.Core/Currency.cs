using System.Runtime.Serialization;

namespace Untech.Practices
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Untech.Practices.Enumeration{Untech.Practices.Currency, System.String}" />
	[DataContract]
	public class Currency : Enumeration<Currency, string>
	{
		protected Currency() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="Currency"/> class.
		/// </summary>
		/// <param name="code">The currency code. Can be accessed via <see cref="Enumeration{TSelf, TKey}.Id"/></param>
		/// <param name="name">The currency name.</param>
		public Currency(string code, string name)
			: base(code, name)
		{

		}
	}
}