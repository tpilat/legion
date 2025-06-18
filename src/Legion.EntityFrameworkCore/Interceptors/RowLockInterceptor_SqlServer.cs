using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace Legion.EntityFrameworkCore.Interceptors;

public partial class RowLockInterceptor_SqlServer : DbCommandInterceptor
{
	public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
	{
		if (eventData.CommandSource == CommandSource.LinqQuery)
			ModifyCommandBasedOnTag(command);

		return base.ReaderExecuting(command, eventData, result);
	}

	private static void ModifyCommandBasedOnTag(DbCommand command)
	{
		var commandText = command.CommandText;
		if (commandText.Contains("WITH ("))
			return;

		var tagMatch = TagRegex().Match(commandText);
		if (!tagMatch.Success)
			return;

		var tag = tagMatch.Groups[1].Value.ToUpperInvariant();
		string? lockHintText = tag switch
		{
			nameof(RowLockHints.LEGION_FOR_UPDATE) => "WITH (UPDLOCK)",
			nameof(RowLockHints.LEGION_NOWAIT) => "WITH (UPDLOCK, NOWAIT)",
			nameof(RowLockHints.LEGION_SKIP_LOCKED) => "WITH (UPDLOCK, READPAST)",
			nameof(RowLockHints.LEGION_FOR_SHARE) => "WITH (ROWLOCK, HOLDLOCK)",
			_ => null
		};

		if (lockHintText != null)
			command.CommandText = CommandTextReplaceRegex().Replace(commandText, $"FROM $1 {lockHintText}");
	}

	[GeneratedRegex(@"\/\*\s*(LEGION_FOR_UPDATE|LEGION_NOWAIT|LEGION_SKIP_LOCKED|LEGION_FOR_SHARE)\s*\*\/", RegexOptions.IgnoreCase, "en-US")]
	private static partial Regex TagRegex();

	[GeneratedRegex(@"FROM (\S+)", RegexOptions.IgnoreCase, "en-US")]
	private static partial Regex CommandTextReplaceRegex();
}
