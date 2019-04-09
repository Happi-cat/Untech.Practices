using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Untech.AsyncCommandEngine.Metadata
{
	public class BuiltInRequestMetadataProviderTest
	{
		private readonly IRequestMetadataProvider _provider;

		public BuiltInRequestMetadataProviderTest()
		{
			_provider = new BuiltInRequestMetadataProvider(new[] { typeof(BuiltInRequestMetadataProviderTest).Assembly });
		}

		[Fact]
		public void Ctor_Fails_WhenArgumentIsNull()
		{
			Assert.Throws<ArgumentNullException>(() => new BuiltInRequestMetadataProvider(null));
		}

		[Fact]
		public void Ctor_Completes_WhenArgumentIsEmpty()
		{
			var provider = new BuiltInRequestMetadataProvider(Enumerable.Empty<Assembly>());

			Assert.NotNull(provider);
		}

		[Fact]
		public void GetMetadata_ThrowsArgumentNotNull_WhenRequestNameIsNull()
		{
			Assert.Throws<ArgumentNullException>(() => _provider.GetMetadata(null));
		}

		[Fact]
		public void GetMetadata_ReturnsBlankMetadata_WhenRequestNotFound()
		{
			var metadata = _provider.GetMetadata("RequestNotFound");

			Assert.NotNull(metadata);
			Assert.Equal(NullRequestMetadata.Instance, metadata);
		}

		[Fact]
		public void GetMetadata_ReturnsMetadata_WhenRequestFound()
		{
			var metadata = _provider.GetMetadata("Untech.AsyncCommandEngine.Metadata.FakeRequest");

			Assert.NotNull(metadata);
			Assert.NotEqual(NullRequestMetadata.Instance, metadata);
		}

		[Fact]
		public void GetAttribute_ReturnsNull_WhenAttributeNotFound()
		{
			var metadata = _provider.GetMetadata("Untech.AsyncCommandEngine.Metadata.FakeRequest");

			Assert.Null(metadata.GetAttribute<ObsoleteAttribute>());
		}

		[Fact]
		public void GetAttribute_ReturnsAttribute_WhenAttributeFoundOnRequestMetadataSource()
		{
			var metadata = _provider.GetMetadata("Untech.AsyncCommandEngine.Metadata.FakeRequest");

			var attribute = metadata.GetAttribute<WatchDogTimeoutAttribute>();

			Assert.NotNull(attribute);
		}

		[Fact]
		public void GetAttribute_ReturnsAttribute_WhenAttributeFoundOnHandler()
		{
			var metadata = _provider.GetMetadata("Untech.AsyncCommandEngine.Metadata.FakeRequest");

			var attribute = metadata.GetAttribute<ThrottleGroupAttribute>();

			Assert.NotNull(attribute);
		}
	}
}