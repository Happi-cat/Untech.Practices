using System;
using System.ComponentModel.DataAnnotations;

namespace Untech.AsyncCommandEngine.Builder
{
	public static class OptionsBuilder
	{
		public static T ConfigureAndValidate<T>(Action<T> configureOptions)
			where T : class, new()
		{
			return ConfigureAndValidate(new T(), configureOptions);
		}

		private static T ConfigureAndValidate<T>(T initialValue, Action<T> configureOptions)
		{
			if (initialValue == null) throw new ArgumentNullException(nameof(initialValue));
			if (configureOptions == null) throw new ArgumentNullException(nameof(configureOptions));

			var option = initialValue;
			configureOptions(option);
			Validate(option);

			return option;
		}

		private static void Validate(Object value)
		{
			var validationContext = new ValidationContext(value);
			Validator.ValidateObject(value, validationContext, true);
		}
	}
}