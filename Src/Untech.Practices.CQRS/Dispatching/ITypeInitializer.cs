namespace Untech.Practices.CQRS.Dispatching
{
	public interface ITypeInitializer
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="request"></param>
		void Init(object handler, object request);
	}
}