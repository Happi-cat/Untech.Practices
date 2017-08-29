namespace Untech.Practices
{
	public class Currency : Enumeration<Currency, string>
	{
		public Currency(string code, string name) 
			: base(code, name) 
		{

		}
	}
}