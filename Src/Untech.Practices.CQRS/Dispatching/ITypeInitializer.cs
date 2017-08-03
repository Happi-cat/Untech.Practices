namespace Untech.Practices.CQRS.Dispatching
{
	public interface ITypeInitializer
	{
		void Init(object handler, object args);
	}
}