using System.Collections.Generic;
using Untech.AsyncJob.Metadata.Annotations;

namespace AsyncJob.Run.Commands
{
	public class DemoCommandBase
	{
		public List<MetadataAttribute> Meta { get; set; }
	}
}
