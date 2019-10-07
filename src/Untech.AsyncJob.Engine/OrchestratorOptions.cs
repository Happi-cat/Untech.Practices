using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Untech.AsyncJob
{
	public class OrchestratorOptions : IValidatableObject
	{
		public OrchestratorOptions()
		{
			Warps = 5;
			RequestsPerWarp = 5;
			SlidingRadius = 6;
			SlidingStep = TimeSpan.FromSeconds(10);
		}

		[Range(1, 50)]
		public int Warps { get; set; }

		[Range(1, 50)]
		public int RequestsPerWarp { get; set; }

		public int SlidingRadius { get; set; }
		public TimeSpan SlidingStep { get; set; }

		public bool RunRequestsInWarpAllAtOnce { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (SlidingStep <= TimeSpan.Zero)
				yield return new ValidationResult("SlidingStep cannot be less or equal to 0");

			if (TimeSpan.FromHours(1) < SlidingStep)
				yield return new ValidationResult("SlidingStep cannot be greater than 1h");

			if (SlidingRadius < 0)
				yield return new ValidationResult("Sliding radius cannot be less than 0");

			if (TimeSpan.FromHours(6).Ticks <= SlidingStep.Ticks * SlidingRadius)
				yield return new ValidationResult("(SlidingRadius * SlidingStep) cannot be greater than 6h");
		}
	}
}
