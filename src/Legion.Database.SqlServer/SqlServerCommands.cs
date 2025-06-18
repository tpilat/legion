using Microsoft.Data.SqlClient;

namespace Legion.Database.SqlServer;

public static class SqlServerCommands
{
	public static async Task<bool> ExistsAsync(SqlConnection connection, SqlTransaction? transaction, string tableName, CancellationToken cancellationToken = default)
	{
		Throw.IfArgumentNull(connection);
		Throw.IfArgumentNullOrWhiteSpace(tableName);

		var cmd = new SqlCommand($"SELECT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = '{tableName}')", connection);
		if (transaction != null)
			cmd.Transaction = transaction;

		var existsObj = await cmd.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);

		if (existsObj is bool exists)
		{
			return exists;
		}
		else
		{
			throw new InvalidOperationException($"Invalid {nameof(existsObj)} = {existsObj}");
		}
	}

	public static async Task<bool> ExistsAsync(SqlConnection connection, SqlTransaction? transaction, string schemaName, string tableName, CancellationToken cancellationToken = default)
	{
		Throw.IfArgumentNull(connection);
		Throw.IfArgumentNullOrWhiteSpace(schemaName);
		Throw.IfArgumentNullOrWhiteSpace(tableName);

		var cmd = new SqlCommand($"SELECT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_schema = '{schemaName}' AND table_name = '{tableName}')", connection);
		if (transaction != null)
			cmd.Transaction = transaction;

		var existsObj = await cmd.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);

		if (existsObj is bool exists)
		{
			return exists;
		}
		else
		{
			throw new InvalidOperationException($"Invalid {nameof(existsObj)} = {existsObj}");
		}
	}

	public static string GetRandomTmpTableName()
		=> $"tmp_{Guid.NewGuid():N}";

	public static Task<string> CopyTableAsTempIfNotExistsAsync(
		SqlConnection connection,
		SqlTransaction? transaction,
		string sourceSchemaName,
		string sourceTableName,
		TmpTableCommitOptions commitOptions,
		bool copyWithData,
		bool truncateDataIfAny,
		CancellationToken cancellationToken = default)
		=> CopyTableAsTempIfNotExistsAsync(connection, transaction, sourceSchemaName, sourceTableName, GetRandomTmpTableName(), commitOptions, copyWithData, truncateDataIfAny, cancellationToken);

	public static async Task<string> CopyTableAsTempIfNotExistsAsync(
		SqlConnection connection,
		SqlTransaction? transaction,
		string sourceSchemaName,
		string sourceTableName,
		string tmpTableName,
		TmpTableCommitOptions commitOptions,
		bool copyWithData,
		bool truncateDataIfAny,
		CancellationToken cancellationToken = default)
	{
		Throw.IfArgumentNull(connection);

		var exists = await ExistsAsync(connection, transaction, tmpTableName, cancellationToken).ConfigureAwait(false);
		if (exists)
		{
			if (truncateDataIfAny)
			{
				await TruncateTableAsync(connection, transaction, tmpTableName, false, cancellationToken).ConfigureAwait(false);
			}

			return tmpTableName;
		}

		exists = await ExistsAsync(connection, transaction, sourceSchemaName, sourceTableName, cancellationToken).ConfigureAwait(false);
		if (!exists)
			throw new InvalidOperationException($"Source {sourceSchemaName}.{sourceTableName} does not exists");

		var options = commitOptions switch
		{
			TmpTableCommitOptions.PreserveRows => "on commit preserve rows",
			TmpTableCommitOptions.DeleteRows => "on commit delete rows",
			TmpTableCommitOptions.Drop => "on commit drop",
			_ => throw new ArgumentException($"Invalid {nameof(commitOptions)}")
		};

		var cloneCommandText = @$" 
SET client_min_messages TO WARNING;

CREATE TEMPORARY TABLE ""{tmpTableName}"" {options}
AS
SELECT * FROM {sourceSchemaName}.""{sourceTableName}""{(copyWithData ? "" : " WHERE 1 = 2")};
";
		using var cloneCommand = new SqlCommand(cloneCommandText, connection);
		if (transaction != null)
			cloneCommand.Transaction = transaction;

		await cloneCommand.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);

		return tmpTableName;
	}

	public static Task TruncateTableAsync(SqlConnection connection, SqlTransaction? transaction, string tableName, bool cascade, CancellationToken cancellationToken = default)
	{
		Throw.IfArgumentNull(connection);
		Throw.IfArgumentNullOrWhiteSpace(tableName);

		using var truncCommand = new SqlCommand(@$"TRUNCATE TABLE ""{tableName}""{(cascade ? " CASCADE" : "")}", connection);
		if (transaction != null)
			truncCommand.Transaction = transaction;

		return truncCommand.ExecuteNonQueryAsync(cancellationToken);
	}

	public static Task TruncateTableAsync(SqlConnection connection, SqlTransaction? transaction, string schemaName, string tableName, bool cascade, CancellationToken cancellationToken = default)
	{
		Throw.IfArgumentNull(connection);
		Throw.IfArgumentNullOrWhiteSpace(schemaName);
		Throw.IfArgumentNullOrWhiteSpace(tableName);

		using var truncCommand = new SqlCommand(@$"TRUNCATE TABLE {schemaName}.""{tableName}""{(cascade ? " CASCADE" : "")}", connection);
		if (transaction != null)
			truncCommand.Transaction = transaction;

		return truncCommand.ExecuteNonQueryAsync(cancellationToken);
	}

	public static Task DropTableAsync(SqlConnection connection, SqlTransaction? transaction, string tableName, CancellationToken cancellationToken = default)
	{
		Throw.IfArgumentNull(connection);
		Throw.IfArgumentNullOrWhiteSpace(tableName);

		using var dropCommand = new SqlCommand(@$"DROP TABLE ""{tableName}""", connection);
		if (transaction != null)
			dropCommand.Transaction = transaction;

		return dropCommand.ExecuteNonQueryAsync(cancellationToken);
	}

	public static Task DropTableAsync(SqlConnection connection, SqlTransaction? transaction, string schemaName, string tableName, CancellationToken cancellationToken = default)
	{
		Throw.IfArgumentNull(connection);
		Throw.IfArgumentNullOrWhiteSpace(schemaName);
		Throw.IfArgumentNullOrWhiteSpace(tableName);

		using var dropCommand = new SqlCommand(@$"DROP TABLE {schemaName}.""{tableName}""", connection);
		if (transaction != null)
			dropCommand.Transaction = transaction;

		return dropCommand.ExecuteNonQueryAsync(cancellationToken);
	}
}
