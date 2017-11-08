using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Untech.Practices.DataStorage
{
	public interface IDataStorage<T, TKey> 
		where T: IAggregateRoot<TKey>
	{
		T Find(TKey key);

		T Create(T entity);

		T Update(T entity);

		bool Delete(T entity);
	}

	public interface IDataStorage<T> : IDataStorage<T, int>
		where T: IAggregateRoot 
	{

	}
}