using System;

namespace Untech.Practices.ObjectPool
{
	internal static class Utils
	{
		public static void TryDispose(object obj)
		{
			if (obj is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}
	}
}