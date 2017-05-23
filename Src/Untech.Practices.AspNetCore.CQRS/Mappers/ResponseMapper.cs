using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Untech.Practices.AspNetCore.CQRS.Mappers.Accessors;

namespace Untech.Practices.AspNetCore.CQRS.Mappers
{
	public class ResponseMapper<T> : IResponseMapper<T>
	{
		private readonly IList<IPropertyBinder<T, HttpResponse>> _binders = new List<IPropertyBinder<T, HttpResponse>>();

		public void Body()
		{
			AddBinder(new InstanceGetAccessor<T>(), new BodySetAccessor<T>());
		}

		public void Body<TProp>(Expression<Func<T, TProp>> member)
		{
			AddBinder(member, new BodySetAccessor<TProp>());
		}

		public void Header<TProp>(Expression<Func<T, TProp>> member, string headerName, Formatter<TProp> formatter)
		{
			AddBinder(member, new HeaderSetAccessor<TProp>(headerName, formatter));
		}

		public void Header(Expression<Func<T, string>> member, string headerName)
		{
			Header(member, headerName, n => n);
		}

		public void Cookie<TProp>(Expression<Func<T, TProp>> member, string cookieName, Formatter<TProp> formatter)
		{
			AddBinder(member, new CookieSetAccessor<TProp>(cookieName, formatter));
		}

		public void Cookie(Expression<Func<T, string>> member, string cookieName)
		{
			Cookie(member, cookieName, n => n);
		}

		public override string ToString() => string.Join(", ", _binders.Select(n => n.ToString("D", null)));

		void IResponseMapper<T>.Map(T input, HttpResponse response)
		{
			foreach (var binder in _binders)
			{
				binder.Bind(input, response);
			}
		}

		private void AddBinder<TProp>(Expression<Func<T, TProp>> sourceMember, ISetAccessor<HttpResponse, TProp> destAccessor)
		{
			AddBinder(MemberAccessor<T, TProp>.Create(sourceMember), destAccessor);
		}

		private void AddBinder<TProp>(IGetAccessor<T, TProp> sourceAccessor, ISetAccessor<HttpResponse, TProp> destAccessor)
		{
			_binders.Add(new ResponsePropertyBinder<T, TProp>(sourceAccessor, destAccessor));
		}
	}
}
