using System;
using System.Collections.Generic;
using System.IO;

namespace Untech.AsyncCommandEngine.Fakes
{
	public class FakeRequest : Request
	{
		private readonly object _body;
		private readonly IEnumerable<Attribute> _meta;

		public FakeRequest(string name = null, object body = null, DateTimeOffset? created = null, IEnumerable<Attribute> meta = null)
		{
			_body = body;
			_meta = meta;

			Identifier = Guid.NewGuid().ToString();
			Name = body?.GetType().FullName ?? nameof(FakeRequest);
			Created = DateTimeOffset.Now;
			Attributes=  new Dictionary<string, string>();
		}

		public override string Identifier { get; }
		public override string Name { get; }
		public override DateTimeOffset Created { get; }
		public override IDictionary<string, string> Attributes { get; }
		public override object GetBody(Type requestType)
		{
			return _body;
		}

		public override Stream GetRawBody()
		{
			throw new NotImplementedException();
		}

		public override IEnumerable<Attribute> GetAttachedMetadata()
		{
			return _meta ?? base.GetAttachedMetadata();
		}
	}
}
