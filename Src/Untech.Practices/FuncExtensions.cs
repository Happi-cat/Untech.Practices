using System;

namespace Untech.Practices
{
	public static class FuncExtensions
	{
		public static Func<TResult> Bind<T, TResult>(this Func<T, TResult> func, T arg)
		{
			return () => func(arg);
		}

		public static Func<T2, TResult> Bind<T1, T2, TResult>(this Func<T1, T2, TResult> func, T1 arg)
		{
			return t2 => func(arg, t2);
		}

		public static Func<T2, T3, TResult> Bind<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func, T1 arg)
		{
			return (t2, t3) => func(arg, t2, t3);
		}

		public static Func<T1, TResult> RightBind<T1, T2, TResult>(this Func<T1, T2, TResult> func, T2 arg)
		{
			return t1 => func(t1, arg);
		}

		public static Func<T1, T2, TResult> RightBind<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func, T3 arg)
		{
			return (t1, t2) => func(t1, t2, arg);
		}

		public static Action Bind<T>(this Action<T> action, T arg)
		{
			return () => action(arg);
		}

		public static Action<T2> Bind<T1, T2>(this Action<T1, T2> action, T1 arg)
		{
			return t2 => action(arg, t2);
		}

		public static Action<T2, T3> Bind<T1, T2, T3>(this Action<T1, T2, T3> action, T1 arg)
		{
			return (t2, t3) => action(arg, t2, t3);
		}

		public static Action<T1> RightBind<T1, T2>(this Action<T1, T2> action, T2 arg)
		{
			return t1 => action(t1, arg);
		}

		public static Action<T1, T2> RightBind<T1, T2, T3>(this Action<T1, T2, T3> action, T3 arg)
		{
			return (t1, t2) => action(t1, t2, arg);
		}
	}
}