using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Untech.AsyncCommandEngine.Features.Throttling
{
	/// <summary>
	/// Options that can be used for throttling configuration.
	/// </summary>
	public class ThrottleOptions : IValidatableObject
	{
		/// <summary>
		/// Gets or sets amount of requests that can be run at the same time.
		/// </summary>
		public int? RunAtOnce { get; set; }

		/// <summary>
		/// Gets or sets amount of requests in group that can be run at the same time.
		/// Group is a throttling group or request.
		/// </summary>
		public int? DefaultRunAtOnceInGroup { get; set; }

		/// <summary>
		/// Gets or sets key/value collection that contains information about requests that can be run at once in group.
		/// Group is a throttling group or request.
		/// </summary>
		public ReadOnlyDictionary<string, int> RunAtOncePerGroups { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (RunAtOnce <= 0)
				yield return new ValidationResult("RunAtOnce cannot be less or equal than zero.");

			if (DefaultRunAtOnceInGroup <= 0)
				yield return new ValidationResult("DefaultRunAtOnceInGroup cannot be less or equal than zero.");

			if (RunAtOncePerGroups == null) yield break;

			foreach (var runAtOncePerGroup in RunAtOncePerGroups)
			{
				if (runAtOncePerGroup.Value > 0) continue;

				yield return new ValidationResult(
					$"Non-positive run at once was configured for {runAtOncePerGroup.Key}"
				);
			}
		}
	}
}