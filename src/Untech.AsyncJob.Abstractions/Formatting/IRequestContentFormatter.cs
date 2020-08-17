using System;
using JetBrains.Annotations;

namespace Untech.AsyncJob.Formatting
{
	public interface IRequestContentFormatter
	{
		string Type { get; }

		string Serialize([CanBeNull] object payload);

		object Deserialize([CanBeNull] string payload, [NotNull] Type type);
	}
}