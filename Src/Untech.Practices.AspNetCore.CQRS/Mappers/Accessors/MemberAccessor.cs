using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Untech.Practices.AspNetCore.CQRS.Mappers.Accessors
{
	internal class MemberAccessor<T, TProp> : IGetAccessor<T, TProp>, ISetAccessor<T, TProp>
	{
		private readonly PropertyInfo _propertyInfo;
		private readonly Func<T, TProp> _getter;
		private readonly Action<T, TProp> _setter;

		public MemberAccessor(PropertyInfo propertyInfo)
		{
			_propertyInfo = propertyInfo;
		}

		public static MemberAccessor<T, TProp> Create(Expression<Func<T, TProp>> memberExpression)
		{
			var member = (MemberExpression)memberExpression.Body;
			return new MemberAccessor<T, TProp>((PropertyInfo)member.Member);
		}

		public TProp Get(T instance) => _getter(instance);

		public void Set(T instance, TProp value) => _setter(instance, value);

		public override string ToString() => $"Prop({_propertyInfo.Name}";

		private static Action<T, TProp> CreateSetter(Type declaringType, string memberName, Type propertyType)
		{
			var objectParameter = Expression.Parameter(typeof(T), "object");
			var valueParameter = Expression.Parameter(typeof(TProp), "value");

			var propertyExpression = Expression.PropertyOrField(Expression.Convert(objectParameter, declaringType), memberName);

			var assignExpression = Expression.Assign(propertyExpression, Expression.Convert(valueParameter, propertyType));

			return Expression
				.Lambda<Action<T, TProp>>(assignExpression, objectParameter, valueParameter)
				.Compile();
		}

		private static Func<T, TProp> CreateGetter(Type declaringType, string memberName)
		{
			var objectParameter = Expression.Parameter(typeof(T), "object");

			var propertyExpression = Expression.PropertyOrField(Expression.Convert(objectParameter, declaringType), memberName);

			return Expression.Lambda<Func<T, TProp>>(Expression.Convert(propertyExpression, typeof(TProp)), objectParameter)
				.Compile();
		}
	}
}
