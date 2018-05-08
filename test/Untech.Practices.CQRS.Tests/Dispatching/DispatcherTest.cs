using System;
using Xunit;

namespace Untech.Practices.CQRS.Dispatching
{
	public class DispatcherTest
	{
		private Dispatcher _dispatcher;

		public DispatcherTest()
		{
			_dispatcher = new Dispatcher(new DummyCQRS.Resolver());
		}

		[Fact]
		public void Process_ThrownInvalidOperation_WhenHandlerNotFound()
		{
			Assert.Throws(typeof(InvalidOperationException), () => _dispatcher.Process(new DummyCQRS.CommandWithUnknownHandler()));
		}

		[Fact]
		public void Fetch_ThrowArgumentNull_WhenArgIsNull()
		{
			Assert.Throws(typeof(ArgumentNullException), () => _dispatcher.Fetch<int>(null));
		}

		[Fact]
		public void Fetch_Returns_WhenHandlerResolved()
		{
			var res = _dispatcher.Fetch(new DummyCQRS.Query());

			Assert.Equal(1, res);
		}

		[Fact]
		public void FetchAsync_ThrowArgumentNull_WhenArgIsNull()
		{
			Assert.Throws(typeof(ArgumentNullException), () => _dispatcher.FetchAsync<int>(null, new System.Threading.CancellationToken()));
		}

		[Fact]
		public void FetchAsync_Returns_WhenHandlerResolved()
		{
			var res = _dispatcher
				.FetchAsync(new DummyCQRS.Query(), new System.Threading.CancellationToken())
				.GetAwaiter()
				.GetResult();

			Assert.Equal(1, res);
		}

		[Fact]
		public void Process_ThrowArgumentNull_WhenArgIsNull()
		{
			Assert.Throws(typeof(ArgumentNullException), () => _dispatcher.Process<int>(null));
		}

		[Fact]
		public void Process_Returns_WhenHandlerResolved()
		{
			var res = _dispatcher.Process(new DummyCQRS.Command());

			Assert.Equal(1, res);
		}

		[Fact]
		public void ProcessAsync_ThrowArgumentNull_WhenArgIsNull()
		{
			Assert.Throws(typeof(ArgumentNullException), () => _dispatcher.ProcessAsync<int>(null, new System.Threading.CancellationToken()));
		}

		[Fact]
		public void ProcessAsync_Returns_WhenHandlerResolved()
		{
			var res = _dispatcher
				.ProcessAsync(new DummyCQRS.Command(), new System.Threading.CancellationToken())
				.GetAwaiter()
				.GetResult();

			Assert.Equal(1, res);
		}

		[Fact]
		public void Publish_ThrowArgumentNull_WhenArgIsNull()
		{
			Assert.Throws(typeof(ArgumentNullException), () => _dispatcher.Publish(null));
		}

		[Fact]
		public void Publish_Returns_WhenHandlerResolved()
		{
			_dispatcher.Publish(new DummyCQRS.Notification());
		}

		[Fact]
		public void PublishAsync_ThrowArgumentNull_WhenArgIsNull()
		{
			Assert.Throws(typeof(ArgumentNullException), () => _dispatcher.PublishAsync(null, new System.Threading.CancellationToken()));
		}

		[Fact]
		public void PublishAsync_Returns_WhenHandlerResolved()
		{
			_dispatcher
				.PublishAsync(new DummyCQRS.Notification(), new System.Threading.CancellationToken())
				.GetAwaiter()
				.GetResult();
		}
	}
}
