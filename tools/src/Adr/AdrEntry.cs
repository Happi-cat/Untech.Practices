using System;
using System.Collections.Generic;

namespace Adr
{
	public class AdrEntry
	{
		public AdrEntry(string title)
		{
			Title = title;
		}

		public string Title { get; private set; }

		public int Number { get; set; }

		public string Status { get; set; }

		public string Context { get; set; }

		public string Decision { get; set; }

		public string Consequences { get; set; }

		public DateTime When { get; set; }

		public static AdrEntry CreateInitial()
		{
			return new AdrEntry("Record architecture decisions")
			{
				Number = 1,
				Status = "Accepted",
				Context =  "We need to record the architectural decisions made on this project.",
				Decision = "We will use Architecture Decision Records, as [described by Michael Nygard](http://thinkrelevance.com/blog/2011/11/15/documenting-architecture-decisions).",
				Consequences = "See Michael Nygard's article, linked above. For a lightweight ADR toolset, see Nat Pryce's [adr-tools](https://github.com/npryce/adr-tools).",
				When = DateTime.Today
			};
		}

		public static AdrEntry CreateNew(string title)
		{
			return new AdrEntry(title)
			{
				Status =  "Proposed",
				Context =  "{context}",
				Decision =  "{decision}",
				Consequences =  "{consequences}",
				When = DateTime.Today
			};
		}

		public AdrEntry AppendStatus(string status)
		{
			if (string.IsNullOrEmpty(Status)) Status = status;

			Status = Status + "\n" + status;

			return this;
		}
	}
}