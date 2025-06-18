using Legion.Database.Metamodel.Info;
using Microsoft.Data.SqlClient;

namespace Legion.Database.SqlServer;

public class TableInfoBulkInsert<T> : ITableInfoBulkInsert<T>, IDisposable
{
	private readonly TableInfo _tableInfo;
	private readonly Func<T, Dictionary<string, object?>> _dictionaryMapper;
	private readonly string? _connectionString;
	private readonly bool _isInternalConnection;

	private SqlConnection? _dbConnection;

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
		SqlConnection dbConnection)
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
	private SqlConnection GetDbConnection()
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

	private SqlConnection CreateDbConnection()
	{
		Throw.IfArgumentNullOrWhiteSpace(_connectionString);

		var dbConnection = new SqlConnection(_connectionString);
		dbConnection.Open();
		return dbConnection;
	}

	public ulong BulkInsert(IEnumerable<T> entities, bool alwaysCreateNewConnection)
	{
		var dbConnection = alwaysCreateNewConnection
			? CreateDbConnection()
			: GetDbConnection();

		try
		{
			var columnTypes = _tableInfo.Columns.ToDictionary(x => x.PropertyName, x => x.DatabaseType);

			ulong result = 0;

			var rows = entities.Select(e => _dictionaryMapper(e)).ToList();
			var dataTable = _tableInfo.ToDataTable(rows);

			using (var bulkCopy = new Microsoft.Data.SqlClient.SqlBulkCopy(dbConnection))
			{
				bulkCopy.DestinationTableName = _tableInfo.FullTableName;

				foreach (var column in _tableInfo.Columns)
					bulkCopy.ColumnMappings.Add(column.PropertyName, column.ColumnName);

				try
				{
					bulkCopy.WriteToServer(dataTable);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}
			}

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
