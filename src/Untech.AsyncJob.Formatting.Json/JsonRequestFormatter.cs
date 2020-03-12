using System;
using System.Text.Json;

namespace Untech.AsyncJob.Formatting.Json
{
	public struct JsonRequestContentFormatter : IRequestContentFormatter
	{
		public static IRequestContentFormatter Default { get; } = new JsonRequestContentFormatter();

		public string Type => "json";

		public string Serialize(object payload)
		{
			return JsonSerializer.Serialize(payload);
		}

		public object Deserialize(string payload, Type type)
		{
			return JsonSerializer.Deserialize(payload, type);
		}
	}
}