namespace Shared.Infrastructure.Data
{
	public interface IUserScopedDao
	{
		int Key { get; }
		int UserKey { get; }
	}
}