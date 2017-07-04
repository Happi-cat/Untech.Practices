using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Untech.Practices.CQRS.Dispatching
{
	[TestClass]
	public class DispatcherTest
	{
		private Dispatcher _dispatcher;

		[TestInitialize]
		public void Init()
		{
			_dispatcher = new Dispatcher(new DummyCQRS.Resolver());
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
			_dispatcher.FetchAsync<int>(null, new System.Threading.CancellationToken());
		}

		[TestMethod]
		public void FetchAsync_Returns_WhenHandlerResolved()
		{
			var res = _dispatcher
				.FetchAsync(new DummyCQRS.Query(), new System.Threading.CancellationToken())
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
			_dispatcher.ProcessAsync<int>(null, new System.Threading.CancellationToken());
		}

		[TestMethod]
		public void ProcessAsync_Returns_WhenHandlerResolved()
		{
			var res = _dispatcher
				.ProcessAsync(new DummyCQRS.Command(), new System.Threading.CancellationToken())
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
			_dispatcher.PublishAsync(null, new System.Threading.CancellationToken());
		}

		[TestMethod]
		public void PublishAsync_Returns_WhenHandlerResolved()
		{
			_dispatcher
				.PublishAsync(new DummyCQRS.Notification(), new System.Threading.CancellationToken())
				.GetAwaiter()
				.GetResult();
		}
	}
}
