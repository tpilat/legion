namespace Legion.EntityFrameworkCore.Expressions.MemberAccess.Tokenizer;

internal static class MemberAccessTokenizer
{
	private static readonly char[] separators = ['.', '['];

	public static IEnumerable<IMemberAccessToken> GetTokens(string memberPath)
	{
		string[] members = memberPath.Split(separators, StringSplitOptions.RemoveEmptyEntries);

		foreach (string member in members)
		{
			if (TryParseIndexerToken(member, out IndexerToken indexerToken))
			{
				yield return indexerToken;
			}
			else
			{
				yield return new PropertyToken(member);
			}
		}
	}

	private static bool TryParseIndexerToken(string member, out IndexerToken token)
	{
		token = null!;

		if (!IsValidIndexer(member))
		{
			return false;
		}

		List<object> arguments = [];
		arguments.AddRange(ExtractIndexerArguments(member).Select(ConvertIndexerArgument));

		token = new IndexerToken(arguments);

		return true;
	}

	private static bool IsValidIndexer(string member)
	{
		return member.EndsWith("]", StringComparison.Ordinal);
	}

	private static IEnumerable<string> ExtractIndexerArguments(string member)
	{
		var args = member.TrimEnd(']');
		foreach (var arg in args.Split(','))
		{
			yield return arg;
		}
	}

	private static object ConvertIndexerArgument(string argument)
	{
		if (int.TryParse(argument, out int argAsInt))
			return argAsInt;

		if (argument.StartsWith("\"", StringComparison.Ordinal))
		{
			return argument.Trim('"');
		}

		if (argument.StartsWith("'", StringComparison.Ordinal))
		{
			var trimmedArg = argument.Trim('\'');
			if (trimmedArg.Length == 1)
			{
				return trimmedArg[0];
			}

			return trimmedArg;
		}

		return argument;
	}
}