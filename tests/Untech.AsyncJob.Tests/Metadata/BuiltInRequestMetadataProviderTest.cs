using System;
using System.Linq;
using System.Reflection;
using Untech.AsyncJob.Features.CQRS;
using Untech.AsyncJob.Metadata.Annotations;
using Xunit;

namespace Untech.AsyncJob.Metadata
{
	public class BuiltInRequestMetadataProviderTest
	{
		private readonly string _commandName = typeof(FakeCommand).FullName;
		private readonly IRequestMetadataProvider _provider;

		public BuiltInRequestMetadataProviderTest()
		{
			_provider = new BuiltInRequestMetadataProvider(new[] { typeof(BuiltInRequestMetadataProviderTest).Assembly });
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
			var metadata = GetMetadata("RequestNotFound");

			Assert.NotNull(metadata);
			Assert.Equal(NullRequestMetadata.Instance, metadata);
		}

		[Fact]
		public void GetMetadata_ReturnsMetadata_WhenRequestFound()
		{
			var metadata = GetMetadata(_commandName);

			Assert.NotNull(metadata);
			Assert.NotEqual(NullRequestMetadata.Instance, metadata);
		}

		[Fact]
		public void GetAttribute_ReturnsNull_WhenAttributeNotFound()
		{
			var metadata = GetMetadata(_commandName);

			Assert.Null(metadata.GetAttribute<FakeAttribute>());
		}

		[Fact]
		public void GetAttribute_ReturnsAttribute_WhenAttributeFoundOnHandler()
		{
			var metadata = GetMetadata(_commandName);

			var attribute = metadata.GetAttribute<ThrottleGroupAttribute>();

			Assert.NotNull(attribute);
		}

		private IRequestMetadata GetMetadata(string requestName)
		{
			return _provider.GetMetadata(new DefaultRequest(requestName));
		}

		private class FakeAttribute : MetadataAttribute { }
	}
}
