using Legion.Database.Metamodel.Info;
using Npgsql;

namespace Legion.Database.PostgreSQL;

public class TableInfoBulkInsert<T> : ITableInfoBulkInsert<T>, IDisposable
{
	private readonly TableInfo _tableInfo;
	private readonly Func<T, Dictionary<string, object?>> _dictionaryMapper;
	private readonly string? _connectionString;
	private readonly bool _isInternalConnection;

	private NpgsqlConnection? _dbConnection;

	public TableInfoBulkInsert(
		TableInfo tableInfo,
		Func<T, Dictionary<string, object?>> dictionaryMapper,
		string connectionString)
	{
		Throw.IfArgumentNull(tableInfo);
		Throw.IfArgumentNull(dictionaryMapper);
		Throw.IfArgumentNullOrWhiteSpace(connectionString);

		_tableInfo = tableInfo;
		_dictionaryMapper = dictionaryMapper;
		_connectionString = connectionString;
		_dbConnection = null;
		_isInternalConnection = true;
	}

	public TableInfoBulkInsert(
		TableInfo tableInfo,
		Func<T, Dictionary<string, object?>> dictionaryMapper,
		NpgsqlConnection? dbConnection)
	{
		Throw.IfArgumentNull(tableInfo);
		Throw.IfArgumentNull(dictionaryMapper);
		Throw.IfArgumentNull(dbConnection);

		_tableInfo = tableInfo;
		_dictionaryMapper = dictionaryMapper;
		_connectionString = null;
		_dbConnection = dbConnection;
		_isInternalConnection = false;
	}

	private readonly object _connectionLock = new();
	private NpgsqlConnection GetDbConnection()
	{
		if (_dbConnection != null)
			return _dbConnection;

		lock (_connectionLock)
		{
			if (_dbConnection != null)
				return _dbConnection;

			_dbConnection = CreateDbConnection();
			return _dbConnection;
		}
	}

	private NpgsqlConnection CreateDbConnection()
	{
		Throw.IfArgumentNullOrWhiteSpace(_connectionString);

		var dbConnection = new NpgsqlConnection(_connectionString);
		dbConnection.Open();
		return dbConnection;
	}

	public ulong BulkInsert(IEnumerable<T> entities, bool alwaysCreateNewConnection)
	{
		string sql = $"COPY {_tableInfo.FullTableName} ({_tableInfo.CommaSeparatedColumns}) FROM STDIN (FORMAT BINARY)";

		var dbConnection = alwaysCreateNewConnection
			? CreateDbConnection()
			: GetDbConnection();

		try
		{
			var columnTypes = _tableInfo.Columns.ToDictionary(x => x.PropertyName, x => x.DatabaseType);

			ulong result = 0;
			var writer = dbConnection.BeginBinaryImport(sql);

			foreach (var entity in entities)
			{
				writer.StartRow();
				var entityDict = _dictionaryMapper(entity);

				foreach (var kvp in entityDict)
					writer.Write(kvp.Value, columnTypes[kvp.Key]);
			}

			result = writer.Complete();

			return result;
		}
		finally
		{
			dbConnection.Dispose();
		}
	}

	private bool _disposed;
	protected virtual void Dispose(bool disposing)
	{
		if (_disposed)
			return;

		_disposed = true;

		if (disposing)
		{
			if (_isInternalConnection)
				_dbConnection?.Dispose();
		}
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
}
