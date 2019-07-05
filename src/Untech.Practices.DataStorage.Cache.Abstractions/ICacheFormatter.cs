namespace Untech.Practices.DataStorage.Cache
{
	public interface ICacheFormatter
	{
		string Serialize(object value);
		T Deserialize<T>(string value);
	}
}