using System;

namespace DependencyDotNet
{
	public static class Wildcard
	{
		public static bool IsMatch(string input, string mask)
		{
			return IsMatch(input.AsSpan(), mask.AsSpan());
		}

		public static bool IsMatch(ReadOnlySpan<char> input, ReadOnlySpan<char> mask)
		{
			if (input.Length == 0 && mask.Length == 0)
				return true;

			char maskChar;
			for (int i = 0; i < mask.Length; i++)
			{
				maskChar = mask[i];

				if (maskChar == '?')
				{
					if (input.Length == 0)
						return false;
					input = input.Slice(1); // ignore current char ang 
				}
				else if (maskChar == '*')
				{
					var maskAfterAsterisk = mask.Slice(i + 1);
					// no need to check remaining input chars, even when input is empty
					if (maskAfterAsterisk.Length == 0)
						return true;

					while (input.Length > 0)
					{
						if (IsMatch(input, maskAfterAsterisk))
							return true;
						input = input.Slice(1); // try next
					}
				}
				else
				{
					if (input.Length == 0 || input[0] != maskChar)
						return false;
					input = input.Slice(1); // move next
				}
			}

			return input.Length == 0;
		}
	}
}
