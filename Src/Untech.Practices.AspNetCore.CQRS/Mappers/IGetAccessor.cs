namespace Untech.Practices.AspNetCore.CQRS.Mappers
{
	internal interface IGetAccessor<in TIn, out TProp>
	{
		TProp Get(TIn instance);
	}
}
