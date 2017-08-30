using System.Runtime.Serialization;

namespace Untech.Practices
{
	[DataContract]
	public class Currency : Enumeration<Currency, string>
	{
		public Currency(string code, string name)
			: base(code, name)
		{

		}
	}
}