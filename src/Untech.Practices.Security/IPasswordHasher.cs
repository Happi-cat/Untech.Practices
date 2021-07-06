namespace Untech.Practices.Security
{
	/// <summary>
	/// Thanks to
	/// </summary>
	public interface IPasswordHasher
	{
		string Hash(string password);

		PasswordCheckResult Check(string hash, string password);
	}
}