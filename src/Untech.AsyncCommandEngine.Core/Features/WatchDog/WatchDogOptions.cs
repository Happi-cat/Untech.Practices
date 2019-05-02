using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Untech.AsyncCommandEngine.Features.WatchDog
{
	/// <summary>
	/// Options that can be used for watch-dog configuration.
	/// </summary>
	public class WatchDogOptions : IValidatableObject
	{
		/// <summary>
		/// Default timeout after which request should be cancelled.
		/// Null is for disabled default timeout
		/// </summary>
		public TimeSpan? DefaultTimeout { get; set; }

		/// <summary>
		/// Gets or sets key/values collection that contains custom timeouts for defined requests names.
		/// </summary>
		public ReadOnlyDictionary<string, TimeSpan> TimeoutPerRequests { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (DefaultTimeout <= TimeSpan.Zero)
				yield return new ValidationResult("DefaultTimeout cannot be less or equal to zero");

			if (TimeoutPerRequests == null) yield break;

			foreach (var timeoutPerRequest in TimeoutPerRequests)
			{
				if (timeoutPerRequest.Value > TimeSpan.Zero) continue;

				yield return new ValidationResult(
					$"Non-positive timeout was configured for {timeoutPerRequest.Key}"
				);
			}
		}
	}
}