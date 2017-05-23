namespace Untech.Practices.AspNetCore.CQRS.Mappers.Accessors
{
	internal class InstanceGetAccessor<T> : IGetAccessor<T, T>
	{
		public T Get(T instance)
		{
			return instance;
		}
	}
}
