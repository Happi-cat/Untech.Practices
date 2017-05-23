namespace Untech.Practices.AspNetCore.CQRS.Mappers
{
	internal interface ISetAccessor<in TIn, in TProp>
	{
		void Set(TIn instance, TProp value);
	}
}
