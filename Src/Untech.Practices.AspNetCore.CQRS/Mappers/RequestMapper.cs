using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Untech.Practices.AspNetCore.CQRS.Mappers.Accessors;

namespace Untech.Practices.AspNetCore.CQRS.Mappers
{
	public class RequestMapper<T> : IRequestMapper<T>
		where T : new()
	{
		private readonly IList<IPropertyBinder<HttpRequest, T>> _binders = new List<IPropertyBinder<HttpRequest, T>>();
		private IGetAccessor<HttpRequest, T> _typeMapper;

		public void Body()
		{
			_typeMapper = new BodyGetAccessor<T>();
		}

		public void Body<TProp>(Expression<Func<T, TProp>> member)
		{
			AddBinder(new BodyGetAccessor<TProp>(), member);
		}

		public void Header<TProp>(string headerName, Expression<Func<T, TProp>> member, Parser<TProp> parser)
		{
			AddBinder(new HeaderGetAccessor<TProp>(headerName, parser), member);
		}

		public void Header(string headerName, Expression<Func<T, string>> member)
		{
			Header(headerName, member, n => n);
		}

		public void Cookie<TProp>(string cookieName, Expression<Func<T, TProp>> member, Parser<TProp> parser)
		{
			AddBinder(new CookieGetAccessor<TProp>(cookieName, parser), member);
		}

		public void Cookie(string cookieName, Expression<Func<T, string>> member)
		{
			Cookie(cookieName, member, n => n);
		}

		public void Path<TProp>(string varName, Expression<Func<T, TProp>> member, Parser<TProp> parser)
		{
			AddBinder(new PathGetAccessor<TProp>(varName, parser), member);
		}

		public void Path(string varName, Expression<Func<T, string>> member)
		{
			Path(varName, member, n => n);
		}

		public void Query<TProp>(string argName, Expression<Func<T, TProp>> member, Parser<TProp> parser)
		{
			AddBinder(new QueryGetAccessor<TProp>(argName, parser), member);
		}

		public void Query(string argName, Expression<Func<T, string>> member)
		{
			Query(argName, member, n => n);
		}

		public override string ToString() => string.Join(", ", _binders.Select(n => n.ToString("S", null)));

		T IRequestMapper<T>.Map(HttpRequest request)
		{
			T output = Construct(request);

			foreach (var binder in _binders)
			{
				binder.Bind(request, output);
			}

			return output;
		}

		private void AddBinder<TProp>(IGetAccessor<HttpRequest, TProp> sourceAccessor, Expression<Func<T, TProp>> destMember)
		{
			_binders.Add(new RequestPropertyBinder<T, TProp>(sourceAccessor, MemberAccessor<T, TProp>.Create(destMember)));
		}

		private T Construct(HttpRequest request)
		{
			if (_typeMapper != null)
			{
				var output = _typeMapper.Get(request);
				return output != null ? output : new T();
			}
			return new T();
		}
	}
}
