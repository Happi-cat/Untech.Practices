using System;
using System.Threading;
using System.Threading.Tasks;
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
		public async Task FetchAsync_ThrowArgumentNull_WhenArgIsNull()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(() => _dispatcher.FetchAsync<int>(null, CancellationToken.None));
		}

		[Fact]
		public async Task FetchAsync_Returns_WhenHandlerResolved()
		{
			var res = await _dispatcher.FetchAsync(new DummyCQRS.Query(), CancellationToken.None);

			Assert.Equal(1, res);
		}

		[Fact]
		public async Task ProcessAsync_ThrowArgumentNull_WhenArgIsNull()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(() => _dispatcher.ProcessAsync<int>(null, CancellationToken.None));
		}

		[Fact]
		public async Task ProcessAsync_Returns_WhenHandlerResolved()
		{
			var res = await _dispatcher.ProcessAsync(new DummyCQRS.Command(), CancellationToken.None);

			Assert.Equal(1, res);
		}

		[Fact]
		public async Task PublishAsync_ThrowArgumentNull_WhenArgIsNull()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(() => _dispatcher.PublishAsync(null, CancellationToken.None));
		}

		[Fact]
		public async Task PublishAsync_Returns_WhenHandlerResolved()
		{
			await _dispatcher.PublishAsync(new DummyCQRS.Event(), CancellationToken.None);
		}

		[Fact]
		public async Task ProcessAsync_Throw_WhenHandlerNotResolved()
		{
			await Assert.ThrowsAsync<InvalidOperationException>(() =>
				_dispatcher.ProcessAsync(new DummyCQRS.CommandWithUnknownHandler(), CancellationToken.None));
		}

		[Fact]
		public async Task ProcessAsync_ThrowException_WhenCommandFails()
		{
			await Assert.ThrowsAsync<NotImplementedException>(() =>
				_dispatcher.ProcessAsync(new DummyCQRS.CommandWithError(), CancellationToken.None));
		}
	}
}
