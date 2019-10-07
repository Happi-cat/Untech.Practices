using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Untech.Practices.CQRS.Dispatching
{
	public class InlineQueueDispatcherTest
	{
		private readonly InlineQueueDispatcher _dispatcher;

		public InlineQueueDispatcherTest()
		{
			_dispatcher = new InlineQueueDispatcher(new Dispatcher(new DummyCQRS.Resolver()));
		}

		[Fact]
		public async Task EnqueueC_ThrowArgumentNull_WhenArgIsNull()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(() => _dispatcher.EnqueueAsync<int>(null));
		}

		[Fact]
		public async Task EnqueueC_Returns_WhenHandlerResolved()
		{
			await _dispatcher.EnqueueAsync(new DummyCQRS.Command());
		}

		[Fact]
		public async Task EnqueueN_ThrowArgumentNull_WhenArgIsNull()
		{
			IEvent msg = null;
			await Assert.ThrowsAsync<ArgumentNullException>(() =>
				_dispatcher.EnqueueAsync(msg, CancellationToken.None));
		}

		[Fact]
		public async Task EnqueueN_Returns_WhenHandlerResolved()
		{
			await _dispatcher.EnqueueAsync(new DummyCQRS.Event(), CancellationToken.None);
		}
	}
}