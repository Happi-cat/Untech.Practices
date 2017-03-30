using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Untech.Practices.CQRS.Dispatching
{
	[TestClass]
	public class DispatcherTest
	{
		private IDispatcher _dispatcher;

		[TestInitialize]
		public void Init()
		{
			_dispatcher = new Dispatcher(new TestResolver());
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Process_ThrownInvalidOperation_WhenHandlerNotFound()
		{
			_dispatcher.Process(new DummyCQRS.CommandWithUnknownHandler());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Fetch_ThrowArgumentNull_WhenArgIsNull()
		{
			_dispatcher.Fetch<int>(null);
		}

		[TestMethod]
		public void Fetch_Returns_WhenHandlerResolved()
		{
			var res = _dispatcher.Fetch(new DummyCQRS.Query());

			Assert.AreEqual(1, res);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void FetchAsync_ThrowArgumentNull_WhenArgIsNull()
		{
			_dispatcher.FetchAsync<int>(null);
		}

		[TestMethod]
		public void FetchAsync_Returns_WhenHandlerResolved()
		{
			var res = _dispatcher
				.FetchAsync(new DummyCQRS.Query())
				.GetAwaiter()
				.GetResult();

			Assert.AreEqual(1, res);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Process_ThrowArgumentNull_WhenArgIsNull()
		{
			_dispatcher.Process<int>(null);
		}

		[TestMethod]
		public void Process_Returns_WhenHandlerResolved()
		{
			var res = _dispatcher.Process(new DummyCQRS.Command());

			Assert.AreEqual(1, res);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ProcessAsync_ThrowArgumentNull_WhenArgIsNull()
		{
			_dispatcher.ProcessAsync<int>(null);
		}

		[TestMethod]
		public void ProcessAsync_Returns_WhenHandlerResolved()
		{
			var res = _dispatcher
				.ProcessAsync(new DummyCQRS.Command())
				.GetAwaiter()
				.GetResult();

			Assert.AreEqual(1, res);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Publish_ThrowArgumentNull_WhenArgIsNull()
		{
			_dispatcher.Publish(null);
		}

		[TestMethod]
		public void Publish_Returns_WhenHandlerResolved()
		{
			_dispatcher.Publish(new DummyCQRS.Notification());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void PublishAsync_ThrowArgumentNull_WhenArgIsNull()
		{
			_dispatcher.PublishAsync(null);
		}

		[TestMethod]
		public void PublishAsync_Returns_WhenHandlerResolved()
		{
			_dispatcher
				.PublishAsync(new DummyCQRS.Notification())
				.GetAwaiter()
				.GetResult();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void EnqueueC_ThrowArgumentNull_WhenArgIsNull()
		{
			_dispatcher.Enqueue<int>(null);
		}

		[TestMethod]
		public void EnqueueC_Returns_WhenHandlerResolved()
		{
			_dispatcher.Enqueue(new DummyCQRS.Command());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void EnqueueN_ThrowArgumentNull_WhenArgIsNull()
		{
			_dispatcher.Enqueue(null);
		}

		[TestMethod]
		public void EnqueueN_Returns_WhenHandlerResolved()
		{
			_dispatcher.Enqueue(new DummyCQRS.Notification());
		}

		private class TestResolver : IHandlersResolver
		{
			public T ResolveHandler<T>() where T : class
			{
				var handler = new DummyCQRS.Handler();
				return handler as T;
			}

			public IEnumerable<T> ResolveHandlers<T>() where T : class
			{
				yield return ResolveHandler<T>();
			}
		}
	}
}
