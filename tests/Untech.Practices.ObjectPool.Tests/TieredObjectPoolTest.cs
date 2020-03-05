using System;
using System.Collections.Generic;
using Microsoft.Extensions.ObjectPool;
using Xunit;

namespace Untech.Practices.ObjectPool
{
	public class TieredObjectPoolTest
	{
		[Fact]
		public void GetAndReturn_SameInstance_WhenDefaultPolicy()
		{
			// Arrange
			var pool = Create(new DefaultPooledObjectPolicy<object>());

			var obj1 = pool.Get();
			pool.Return(obj1);

			// Act
			var obj2 = pool.Get();

			// Assert
			Assert.Same(obj1, obj2);
		}

		[Fact]
		public void GetAndReturn_SameInstance_WhenListPolicy()
		{
			// Arrange
			var pool = Create(new ListPolicy());

			var list1 = pool.Get();
			pool.Return(list1);

			// Act
			var list2 = pool.Get();

			// Assert
			Assert.Same(list1, list2);
		}

		[Fact]
		public void Get_ObjectCreatedByPolicy()
		{
			// Arrange
			var pool = Create(new ListPolicy());

			// Act
			var list = pool.Get();

			// Assert
			Assert.Equal(17, list.Capacity);
		}

		[Fact]
		public void Return_ObjectRejectedByPolicy()
		{
			// Arrange
			var pool = Create(new ListPolicy());
			var list1 = pool.Get();
			list1.Capacity = 20;

			// Act
			pool.Return(list1);
			var list2 = pool.Get();

			// Assert
			Assert.NotSame(list1, list2);
		}

		[Fact]
		public void Dispose_ObjectDisposed_WithOneElement()
		{
			// Arrange
			var pool = Create(new DefaultPooledObjectPolicy<DisposableObject>());
			var obj = pool.Get();
			pool.Return(obj);

			// Act
			pool.Dispose();

			// Assert
			Assert.True(obj.IsDisposed);
		}

		[Fact]
		public void Dispose_ObjectsDisposed_WithTwoElements()
		{
			// Arrange
			var pool = Create(new DefaultPooledObjectPolicy<DisposableObject>());
			var obj1 = pool.Get();
			var obj2 = pool.Get();
			pool.Return(obj1);
			pool.Return(obj2);

			// Act
			pool.Dispose();

			// Assert
			Assert.True(obj1.IsDisposed);
			Assert.True(obj2.IsDisposed);
		}

		[Fact]
		public void Get_ThrowsObjectDisposed_WhenDisposed()
		{
			// Arrange
			var pool = Create(new DefaultPooledObjectPolicy<DisposableObject>());
			var obj1 = pool.Get();
			var obj2 = pool.Get();
			pool.Return(obj1);
			pool.Return(obj2);

			// Act
			pool.Dispose();

			// Assert
			Assert.Throws<ObjectDisposedException>(() => pool.Get());
		}

		[Fact]
		public void Return_DisposesObject_WhenPoolDisposed()
		{
			// Arrange
			var pool = Create(new DefaultPooledObjectPolicy<DisposableObject>());
			var obj = pool.Get();

			// Act
			pool.Dispose();
			pool.Return(obj);

			// Assert
			Assert.True(obj.IsDisposed);
		}

		private static TieredObjectPool<T> Create<T>(IPooledObjectPolicy<T> policy)
			where T : class
		{
			return new TieredObjectPool<T>(policy, 10, new NullObjectPool<T>(policy));
		}

		private class ListPolicy : IPooledObjectPolicy<List<int>>
		{
			public List<int> Create()
			{
				return new List<int>(17);
			}

			public bool Return(List<int> obj)
			{
				return obj.Capacity == 17;
			}
		}

		private class DisposableObject : IDisposable
		{
			public bool IsDisposed { get; private set; }

			public void Dispose() => IsDisposed = true;
		}
	}
}