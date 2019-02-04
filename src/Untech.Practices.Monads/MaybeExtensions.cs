using System;

namespace Untech.Practices.Monads
{
	/// <summary>
	/// </summary>
	public static class MaybeExtensions
	{
		public static Maybe<T> ToJust<T>(this T value)
		{
			return new Maybe<T>(value);
		}

		public static Maybe<T> ToMaybe<T>(this T value)
		{
			return value != null ? new Maybe<T>(value) : Maybe<T>.Nothing;
		}

		public static Maybe<T> Where<T>(this Maybe<T> a, Func<T, bool> predicate)
		{
			return a.HasValue && predicate(a.Value)
				? a
				: Maybe<T>.Nothing;
		}

		public static Maybe<B> Bind<A, B>(this Maybe<A> a, Func<A, Maybe<B>> func)
		{
			return a.HasValue ? func(a.Value) : Maybe<B>.Nothing;
		}

		public static Maybe<C> SelectMany<A, B, C>(this Maybe<A> a, Func<A, Maybe<B>> func, Func<A, B, C> select)
		{
			return a.Bind(aval =>
				func(aval).Bind(bval =>
					select(aval, bval).ToMaybe()));
		}

		public static Maybe<B> Select<A, B>(this Maybe<A> a, Func<A, Maybe<B>> func)
		{
			return a.Bind(func);
		}
	}
}