using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Untech.Practices.CQRS.Dispatching
{
	[TestClass]
	public class SimpleQueueDispatcherTest
	{
		private SimpleQueueDispatcher _dispatcher;

		[TestInitialize]
		public void Init()
		{
			_dispatcher = new SimpleQueueDispatcher();
			_dispatcher.Init(new Dispatcher(new DummyCQRS.Resolver()));
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
	}
}
