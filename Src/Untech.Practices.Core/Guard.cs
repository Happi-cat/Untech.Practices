using System;
using System.Collections;

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

		/// <summary>
		/// Checks object equality to null.
		/// </summary>
		/// <param name="paramName">Parameter name.</param>
		/// <param name="obj">String to validate.</param>
		/// <exception cref="ArgumentNullException"><paramref name="obj"/> is null.</exception>
		public static void CheckNotNullOrEmpty(string paramName, string obj)
		{
			if (!string.IsNullOrEmpty(obj)) return;

			throw new ArgumentNullException("Argument cannot be null or empty", paramName);
		}

		/// <summary>
		/// Checks object equality to null.
		/// </summary>
		/// <param name="paramName">Parameter name.</param>
		/// <param name="obj">String to validate.</param>
		/// <exception cref="ArgumentNullException"><paramref name="obj"/> is null.</exception>
		public static void CheckNotNullOrEmpty(string paramName, IEnumerable obj)
		{
			if (obj != null && obj.GetEnumerator().MoveNext()) return;

			throw new ArgumentNullException("Argument cannot be null or empty", paramName);
		}
	}
}