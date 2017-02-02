using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Untech.Practices.CQRS.Owin
{
	public sealed class UriPattern
	{
		private static readonly Regex Pattern = new Regex(@"\{[^{}]+\}", RegexOptions.Compiled);
		private static readonly Regex RegexMetaPattern = new Regex(@"[\\\[\]\(\)\&\^\$\?\#\+\*\|\>\<]", RegexOptions.Compiled);
		private static string _regexMetaCharactersReplacements = @"\$0";
		private static string _tokenReplacement = "([^/?#]*)?";

		private readonly Regex _p;
		private readonly string[] _tokens;

		private UriPattern(string template, bool compiled)
			: this(template, compiled ? RegexOptions.Compiled : RegexOptions.None)
		{
		}

		private UriPattern(string template, RegexOptions options = RegexOptions.None)
		{
			_tokens = GetTokens(template);
			var finalPattern = BuildRegex(template);
			_p = new Regex(finalPattern, options);
		}

		public IDictionary<string, string> Parse(string instance)
		{
			IDictionary<string, string> dict = new Dictionary<string, string>();

			foreach (Match match in _p.Matches(instance))
			{
				int tokenIndex = 0;
				foreach (Group group in match.Groups)
				{
					if (tokenIndex > 0 && group.Success)
					{
						dict.Add(_tokens[tokenIndex - 1], group.Value);
					}

					tokenIndex++;
				}
			}

			return dict; // make readonly?
		}

		private string BuildRegex(string template)
		{
			template = RegexMetaPattern.Replace(template, _regexMetaCharactersReplacements) + ".*";
			return Pattern.Replace(template, _tokenReplacement);
		}

		private string[] GetTokens(string template)
		{
			List<string> tokens = new List<string>();

			foreach (Match match in Pattern.Matches(template))
			{
				string token = match.Value;
				token = token.Substring(1, token.Length - 2); // chop off the leading and trailing curly brace

				if (!tokens.Contains(token))
					tokens.Add(token);
			}

			return tokens.ToArray();
		}

		public static UriPattern Create(string template)
		{
			return new UriPattern(template);
		}

		public static UriPattern Create(string template, bool compiled)
		{
			return new UriPattern(template, compiled);
		}

		public static UriPattern Create(string template, RegexOptions options)
		{
			return new UriPattern(template, options);
		}
	}
}