using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Untech.Practices.AspNetCore.CQRS.Mappers
{
	internal interface IPropertyBinder<TIn, TOut> : IFormattable
	{
		void Bind(TIn sourceContainer, TOut destContainer);
	}
}
