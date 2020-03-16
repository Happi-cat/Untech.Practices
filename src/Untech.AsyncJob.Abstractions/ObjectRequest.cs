using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Untech.AsyncJob.Formatting;

namespace Untech.AsyncJob
{
	public class ObjectRequest : Request
	{
		private readonly object _content;
		private string _name;
		private string _serializedContent;
		private readonly IRequestContentFormatter _formatter;

		public ObjectRequest(
			[NotNull] object content,
			[NotNull] IRequestContentFormatter formatter,
			[CanBeNull] IReadOnlyDictionary<string, string> attributes = null)
			: this(Guid.NewGuid().ToString(), DateTimeOffset.Now, content, formatter, attributes)
		{

		}

		public ObjectRequest(
			[NotNull] string name,
			[NotNull] object content,
			[NotNull] IRequestContentFormatter formatter,
			[CanBeNull] IReadOnlyDictionary<string, string> attributes = null)
			: this(Guid.NewGuid().ToString(), name, DateTimeOffset.Now, content, formatter, attributes)
		{

		}

		public ObjectRequest(
			[NotNull] string identifier,
			DateTimeOffset created,
			[NotNull] object content,
			[NotNull] IRequestContentFormatter formatter,
			[CanBeNull] IReadOnlyDictionary<string, string> attributes = null)
		{
			_content = content ?? throw new ArgumentNullException(nameof(content));
			_formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));

			Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
			Created = created;
			Attributes = attributes;
		}

		public ObjectRequest(
			[NotNull] string identifier,
			[NotNull] string name,
			DateTimeOffset created,
			[NotNull] object content,
			[NotNull] IRequestContentFormatter formatter,
			[CanBeNull] IReadOnlyDictionary<string, string> attributes = null)
		{
			_name = name ?? throw new ArgumentNullException(name);
			_content = content ?? throw new ArgumentNullException(nameof(content));
			_formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));

			Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
			Created = created;
			Attributes = attributes;
		}

		public override string Identifier { get; }

		public override string Name
		{
			get
			{
				return _name = _name ?? _content.GetType().FullName ?? "<unknown>";
			}
		}

		public override DateTimeOffset Created { get; }
		public override IReadOnlyDictionary<string, string> Attributes { get; }

		public override string Content
		{
			get
			{
				return _serializedContent = _serializedContent ?? _formatter.Serialize(_content);
			}
		}

		public override string ContentType
		{
			get
			{
				return _formatter.Type;
			}
		}
	}
}