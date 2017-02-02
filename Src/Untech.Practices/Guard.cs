using System;

namespace Untech.Practices
{
	public static class Guard
	{
		/// <summary>
		/// Checks object equality to null.
		/// </summary>
		/// <param name="paramName">Parameter name.</param>
		/// <param name="obj">Object to validate.</param>
		/// <exception cref="ArgumentNullException"><paramref name="obj"/> is null.</exception>
		public static void CheckNotNull(string paramName, object obj)
		{
			if (obj != null) return;

			throw new ArgumentNullException(paramName);
		}
	}
}