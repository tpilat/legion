using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace Legion.EntityFrameworkCore.Interceptors;

/// <summary>
/// Interceptor to modify SQL commands to include row lock hints for PostgreSQL.
/// </summary>
public partial class RowLockInterceptor_PostgreSql : DbCommandInterceptor
{
	/// <summary>
	/// Intercepts the execution of a command that returns a <see cref="DbDataReader"/>.
	/// </summary>
	/// <param name="command">The command being executed.</param>
	/// <param name="eventData">Contextual information about the command.</param>
	/// <param name="result">The result of the command execution.</param>
	/// <returns>The result of the command execution, possibly modified.</returns>
	public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
	{
		if (eventData.CommandSource == CommandSource.LinqQuery)
			ModifyCommandBasedOnTag(command);

		return base.ReaderExecuting(command, eventData, result);
	}

	/// <summary>
	/// Modifies the command text to include the appropriate row lock hint based on a tag.
	/// </summary>
	/// <param name="command">The command to modify.</param>
	private static void ModifyCommandBasedOnTag(DbCommand command)
	{
		var tagMatch = TagRegex().Match(command.CommandText);

		if (!tagMatch.Success)
			return;

		var hint = tagMatch.Groups[1].Value.ToUpperInvariant() switch
		{
			nameof(RowLockHints.LEGION_FOR_UPDATE) => "FOR UPDATE",
			nameof(RowLockHints.LEGION_NOWAIT) => "FOR UPDATE NOWAIT",
			nameof(RowLockHints.LEGION_SKIP_LOCKED) => "FOR UPDATE SKIP LOCKED",
			nameof(RowLockHints.LEGION_FOR_SHARE) => "FOR SHARE",
			_ => null
		};

		if (hint != null)
			command.CommandText = $"{command.CommandText} {hint}";
	}

	/// <summary>
	/// Regular expression to match row lock hint tags in the command text.
	/// </summary>
	/// <returns>A <see cref="Regex"/> object to match row lock hint tags.</returns>
	[GeneratedRegex(@"\/\*\s*(LEGION_FOR_UPDATE|LEGION_NOWAIT|LEGION_SKIP_LOCKED|LEGION_FOR_SHARE)\s*\*\/", RegexOptions.IgnoreCase, "en-US")]
	private static partial Regex TagRegex();
}
