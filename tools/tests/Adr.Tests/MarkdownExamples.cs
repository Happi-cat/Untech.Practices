using System;

namespace Adr
{
	public static class Examples
	{
		public static readonly string InitialRecordMarkdown =@"# 1. Record architecture decisions

Date: 2019-09-19

## Status

Accepted

## Context

We need to record the architectural decisions made on this project.

## Decision

We will use Architecture Decision Records, as [described by Michael Nygard](http://thinkrelevance.com/blog/2011/11/15/documenting-architecture-decisions).

## Consequences

See Michael Nygard's article, linked above. For a lightweight ADR toolset, see Nat Pryce's [adr-tools](https://github.com/npryce/adr-tools).

";

		public static AdrEntry CreateNewLinesAndOmittedSections() =>
			new AdrEntry("Handle new lines and omitted sections")
			{
				Number = 2,
				When = new DateTime(2019, 9, 19),
				Status = "Approved",
				Decision = @"We should test parsing of new lines in sections:
* Sentence per line
* Empty lines between paragraphs 

Tests to be done:
* Read
* Write"
			};

		public static readonly string NewLinesAndOmittedSectionsMarkdown = @"# 2. Handle new lines and omitted sections

Date: 2019-09-19

## Status

Approved

## Decision

We should test parsing of new lines in sections:
* Sentence per line
* Empty lines between paragraphs 

Tests to be done:
* Read
* Write

";
	}
}