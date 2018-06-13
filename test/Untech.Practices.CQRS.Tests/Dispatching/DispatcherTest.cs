using System;
using System.Threading;
using Xunit;

namespace Untech.Practices.CQRS.Dispatching
{
	public class DispatcherTest
	{
		private readonly Dispatcher _dispatcher;

		public DispatcherTest()
		{
			_dispatcher = new Dispatcher(new DummyCQRS.Resolver());
		}

		[Fact]
		public void Fetch_ThrowArgumentNull_WhenArgIsNull()
		{
			Assert.Throws<ArgumentNullException>(() => _dispatcher.Fetch<int>(null));
		}

		[Fact]
		public void Fetch_Returns_WhenHandlerResolved()
		{
			var res = _dispatcher.Fetch(new DummyCQRS.Query());

			Assert.Equal(1, res);
		}

		[Fact]
		public async void FetchAsync_ThrowArgumentNull_WhenArgIsNull()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(() => _dispatcher.FetchAsync<int>(null, CancellationToken.None));
		}

		[Fact]
		public async void FetchAsync_Returns_WhenHandlerResolved()
		{
			var res = await _dispatcher.FetchAsync(new DummyCQRS.Query(), CancellationToken.None);

			Assert.Equal(1, res);
		}

		[Fact]
		public void Process_ThrownInvalidOperation_WhenHandlerNotFound()
		{
			Assert.Throws<InvalidOperationException>(() => _dispatcher.Process(new DummyCQRS.CommandWithUnknownHandler()));
		}

		[Fact]
		public void Process_ThrowArgumentNull_WhenArgIsNull()
		{
			Assert.Throws<ArgumentNullException>(() => _dispatcher.Process<int>(null));
		}

		[Fact]
		public void Process_Returns_WhenHandlerResolved()
		{
			var res = _dispatcher.Process(new DummyCQRS.Command());

			Assert.Equal(1, res);
		}

		[Fact]
		public async void ProcessAsync_ThrowArgumentNull_WhenArgIsNull()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(() => _dispatcher.ProcessAsync<int>(null, CancellationToken.None));
		}

		[Fact]
		public async void ProcessAsync_Returns_WhenHandlerResolved()
		{
			var res = await _dispatcher.ProcessAsync(new DummyCQRS.Command(), CancellationToken.None);

			Assert.Equal(1, res);
		}

		[Fact]
		public void Publish_ThrowArgumentNull_WhenArgIsNull()
		{
			Assert.Throws<ArgumentNullException>(() => _dispatcher.Publish(null));
		}

		[Fact]
		public void Publish_Returns_WhenHandlerResolved()
		{
			_dispatcher.Publish(new DummyCQRS.Notification());
		}

		[Fact]
		public async void PublishAsync_ThrowArgumentNull_WhenArgIsNull()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(() => _dispatcher.PublishAsync(null, CancellationToken.None));
		}

		[Fact]
		public async void PublishAsync_Returns_WhenHandlerResolved()
		{
			await _dispatcher.PublishAsync(new DummyCQRS.Notification(), CancellationToken.None);
		}
	}
}
