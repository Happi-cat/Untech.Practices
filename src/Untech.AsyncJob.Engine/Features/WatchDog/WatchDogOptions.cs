﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Untech.AsyncJob.Features.WatchDog
{
	/// <summary>
	/// Options that can be used for watch-dog configuration.
	/// </summary>
	public class WatchDogOptions : IValidatableObject
	{
		public WatchDogOptions()
		{
			TimeoutPerRequests = new Dictionary<string, TimeSpan>();
		}

		/// <summary>
		/// Default timeout after which request should be cancelled.
		/// Null is for disabled default timeout
		/// </summary>
		public TimeSpan? DefaultTimeout { get; set; }

		/// <summary>
		/// Gets or sets key/values collection that contains custom timeouts for defined requests names.
		/// </summary>
		public IDictionary<string, TimeSpan> TimeoutPerRequests { get; private set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (DefaultTimeout <= TimeSpan.Zero)
				yield return new ValidationResult("DefaultTimeout cannot be less or equal to zero");

			if (TimeoutPerRequests == null)
				yield break;

			foreach (var timeoutPerRequest in TimeoutPerRequests)
			{
				if (timeoutPerRequest.Value > TimeSpan.Zero)
					continue;

				yield return new ValidationResult(
					$"Non-positive timeout was configured for {timeoutPerRequest.Key}"
				);
			}
		}
	}
}
