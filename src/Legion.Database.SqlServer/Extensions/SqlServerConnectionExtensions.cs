using Legion.Database.SqlServer;
using Microsoft.Data.SqlClient;
using System.Reflection;

namespace Legion.Extensions;

public static class SqlServerConnectionExtensions
{
	private static readonly Lazy<Func<SqlConnection, SqlTransaction?>?> _transactionGetter = new(() =>
	{
		var type = typeof(SqlConnection);
		var transactionField = type.GetField("_currentTransaction", BindingFlags.Instance | BindingFlags.NonPublic);
		if (transactionField == null)
			return null;

		var getter = Legion.Reflection.Internal.DelegateFactory.CreateGet<SqlConnection, SqlTransaction?>(transactionField);
		return getter!;
	});

	public static Task<bool> ExistsAsync(this SqlConnection connection, SqlTransaction? transaction, string tableName, CancellationToken cancellationToken = default)
		=> SqlServerCommands.ExistsAsync(connection, transaction, tableName, cancellationToken);

	public static Task<bool> ExistsAsync(this SqlConnection connection, SqlTransaction? transaction, string schemaName, string tableName, CancellationToken cancellationToken = default)
		=> SqlServerCommands.ExistsAsync(connection, transaction, schemaName, tableName, cancellationToken);

	public static Task<string> CopyTableAsTempIfNotExistsAsync(
		this SqlConnection connection,
		SqlTransaction? transaction,
		string sourceSchemaName,
		string sourceTableName,
		TmpTableCommitOptions commitOptions = TmpTableCommitOptions.PreserveRows,
		bool copyWithData = false,
		bool truncateDataIfAny = true,
		CancellationToken cancellationToken = default)
		=> SqlServerCommands.CopyTableAsTempIfNotExistsAsync(connection, transaction, sourceSchemaName, sourceTableName, commitOptions, copyWithData, truncateDataIfAny, cancellationToken);

	public static Task<string> CopyTableAsTempIfNotExistsAsync(
		this SqlConnection connection,
		SqlTransaction? transaction,
		string sourceSchemaName,
		string sourceTableName,
		string tmpTableName,
		TmpTableCommitOptions commitOptions,
		bool copyWithData,
		bool truncateDataIfAny,
		CancellationToken cancellationToken = default)
		=> SqlServerCommands.CopyTableAsTempIfNotExistsAsync(connection, transaction, sourceSchemaName, sourceTableName, tmpTableName, commitOptions, copyWithData, truncateDataIfAny, cancellationToken);

	public static Task TruncateTableAsync(this SqlConnection connection, SqlTransaction? transaction, string tableName, bool cascade, CancellationToken cancellationToken = default)
		=> SqlServerCommands.TruncateTableAsync(connection, transaction, tableName, cascade, cancellationToken);

	public static Task TruncateTableAsync(this SqlConnection connection, SqlTransaction? transaction, string schemaName, string tableName, bool cascade, CancellationToken cancellationToken = default)
		=> SqlServerCommands.TruncateTableAsync(connection, transaction, schemaName, tableName, cascade, cancellationToken);

	public static Task DropTableAsync(this SqlConnection connection, SqlTransaction? transaction, string schemaName, string tableName, CancellationToken cancellationToken = default)
		=> SqlServerCommands.DropTableAsync(connection, transaction, schemaName, tableName, cancellationToken);

	public static Task DropTableAsync(this SqlConnection connection, SqlTransaction? transaction, string tableName, CancellationToken cancellationToken = default)
		=> SqlServerCommands.DropTableAsync(connection, transaction, tableName, cancellationToken);

	//private static readonly object _getCurrentTransactionLock = new();
	public static SqlTransaction? GetCurrentTransaction(this SqlConnection connector)
	{
		Throw.IfArgumentNull(connector);

		SqlTransaction? result;

		var getter = _transactionGetter.Value;
		if (getter == null)
			return null;

		result = getter(connector);
		//}

		return result;
	}

	public static bool IsInTransaction(this SqlConnection connection)
	{
		Throw.IfArgumentNull(connection);

		var tran = GetCurrentTransaction(connection);
		return tran != null;
	}
}
