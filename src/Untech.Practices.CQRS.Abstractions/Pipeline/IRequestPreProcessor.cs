﻿using System.Threading.Tasks;
using Untech.Practices.CQRS.Handlers;

namespace Untech.Practices.CQRS.Pipeline
{
	/// <summary>
	///     Defines a pipeline step that will be triggered before <see cref="IRequestHandler{TIn,TOut}" /> or
	///     <see cref="IRequestHandler{TIn,TOut}" />
	/// </summary>
	/// <typeparam name="TRequest">CQRS request type.</typeparam>
	public interface IRequestPreProcessor<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
	{
		/// <summary>
		///     Pre process <paramref name="request" />.
		/// </summary>
		/// <param name="request">CQRS request.</param>
		Task PreProcessAsync(IRequestHandler<TRequest, TResponse> handler, TRequest request);
	}
}