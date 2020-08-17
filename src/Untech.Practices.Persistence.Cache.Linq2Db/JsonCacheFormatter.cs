using System.Text.Json;

namespace Untech.Practices.Persistence.Cache.Linq2Db
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
			return JsonSerializer.Serialize(value, _options);
		}

		public T Deserialize<T>(string value)
		{
			return JsonSerializer.Deserialize<T>(value, _options);
		}
	}
}
