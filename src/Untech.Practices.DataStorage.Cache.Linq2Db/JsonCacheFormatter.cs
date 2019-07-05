using System.Text.Json.Serialization;

namespace Untech.Practices.DataStorage.Cache.Linq2Db
{
	public class JsonCacheFormatter : ICacheFormatter
	{
		private readonly JsonSerializerOptions _options;

		public JsonCacheFormatter(JsonSerializerOptions options = null)
		{
			_options = options;
		}

		public string Serialize(object value)
		{
			return JsonSerializer.ToString(value, _options);
		}

		public T Deserialize<T>(string value)
		{
			return JsonSerializer.Parse<T>(value, _options);
		}
	}
}