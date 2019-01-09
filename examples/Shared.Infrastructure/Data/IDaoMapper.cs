using Untech.Practices.UserContext;

namespace Shared.Infrastructure.Data
{
	public interface IDaoMapper<T, TDao>
	{
		T FromDao(IUserContext userContext, TDao dao);
		TDao ToDao(IUserContext userContext, T entity);
	}
}