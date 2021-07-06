namespace Untech.Practices.Security
{
	public interface ISecretsGenerator
	{
		string GenerateSecret(int bytesCount, bool urlSafe = false);
	}
}