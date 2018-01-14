using System;
using Xunit;

namespace Untech.Practices.CQRS.Dispatching
{
	public class SimpleQueueDispatcherTest
	{
		private SimpleQueueDispatcher _dispatcher;

		public SimpleQueueDispatcherTest()
		{
			_dispatcher = new SimpleQueueDispatcher(new Dispatcher(new DummyCQRS.Resolver()));
		}

		[Fact]
		public void EnqueueC_ThrowArgumentNull_WhenArgIsNull()
		{
			Assert.Throws(typeof(ArgumentNullException), () => _dispatcher.Enqueue<int>(null));
		}

		[Fact]
		public void EnqueueC_Returns_WhenHandlerResolved()
		{
			_dispatcher.Enqueue(new DummyCQRS.Command());
		}

		[Fact]
		public void EnqueueN_ThrowArgumentNull_WhenArgIsNull()
		{
			Assert.Throws(typeof(ArgumentNullException), () => _dispatcher.Enqueue(null));
		}

		[Fact]
		public void EnqueueN_Returns_WhenHandlerResolved()
		{
			_dispatcher.Enqueue(new DummyCQRS.Notification());
		}
	}
}
