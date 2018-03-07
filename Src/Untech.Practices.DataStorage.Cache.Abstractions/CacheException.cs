﻿using System;

namespace Untech.Practices.DataStorage.Cache
{
	/// <summary>
	/// Represents known errors that occur during work with cache.
	/// </summary>
	public class CacheException : Exception
	{
		public CacheException(string message, Exception innerException = null)
			: base(message, innerException)
		{
		}
	}
}