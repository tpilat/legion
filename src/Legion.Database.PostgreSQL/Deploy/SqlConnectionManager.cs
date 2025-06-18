using Npgsql;

namespace Legion.Database.PostgreSQL.Deploy;

public class SqlConnectionManager : IDisposable
{
	private readonly DbDeploySettings _dbDeploySettings;
	private readonly Dictionary<string, NpgsqlConnection> _conncetionsWithoutTransaction;
	private readonly Dictionary<string, NpgsqlConnection> _conncetionsWithTransaction;
	private readonly Dictionary<string, NpgsqlTransaction> _globalTransactions;

	private bool disposed;

	public SqlConnectionManager(DbDeploySettings dbDeploySettings)
	{
		Throw.IfArgumentNull(dbDeploySettings);

		_dbDeploySettings = dbDeploySettings;
		_conncetionsWithoutTransaction = [];
		_conncetionsWithTransaction = [];
		_globalTransactions = [];
	}

	public (NpgsqlConnection NpgsqlConnection, NpgsqlTransaction? NpgsqlTransaction) GetOrOpenConncection(string database, DbDeploySettings.TransactionMode? transactionMode)
	{
		var useGlobalTransaction = false;
		var useLocalTransaction = false;
		NpgsqlTransaction? transaction = null;

		if (!transactionMode.HasValue)
		{
			useGlobalTransaction = false;
			useLocalTransaction = false;
		}
		else if (transactionMode == DbDeploySettings.TransactionMode.Global)
		{
			if (_dbDeploySettings.UseGlobalTransaction)
			{
				useGlobalTransaction = true;
				useLocalTransaction = false;
			}
			else
			{
				useGlobalTransaction = false;
				useLocalTransaction = true;
			}
		}
		else if (transactionMode == DbDeploySettings.TransactionMode.Local)
		{
			useGlobalTransaction = false;
			useLocalTransaction = true;
		}
		else if (transactionMode == DbDeploySettings.TransactionMode.None)
		{
			if (_dbDeploySettings.UseGlobalTransaction)
			{
				useGlobalTransaction = true;
				useLocalTransaction = false;
			}
			else
			{
				useGlobalTransaction = false;
				useLocalTransaction = false;
			}
		}
		else
		{
			Throw.NotSupportedException();
		}

		if (useGlobalTransaction)
		{
			if (_conncetionsWithTransaction.TryGetValue(database, out var globalConnectionWithTransaction))
				return (globalConnectionWithTransaction, _globalTransactions[database]);
		}
		else if (!useLocalTransaction) //none, use shared connection without transaction
		{
			if (_conncetionsWithoutTransaction.TryGetValue(database, out var globalConnectionWithoutTransaction))
				return (globalConnectionWithoutTransaction, null);
		}

		var connectionStringBuilder =
			new NpgsqlConnectionStringBuilder(_dbDeploySettings.ConncetionString)
			{
				Database = database
			};

		var connectionString = connectionStringBuilder.ConnectionString;

		if (connectionString.Contains("{0}"))
			connectionString = string.Format(connectionString, System.Environment.GetEnvironmentVariable("PGPASSWORD"));

		var connection = new NpgsqlConnection(connectionString);
		connection.Open();

		if (useGlobalTransaction)
		{
			_conncetionsWithTransaction.Add(database, connection);
		}
		else if (!useLocalTransaction)
		{
			_conncetionsWithoutTransaction.Add(database, connection);
		}

		if (useGlobalTransaction || useLocalTransaction)
		{
			transaction = connection.BeginTransaction();

			if (useGlobalTransaction)
				_globalTransactions.Add(database, transaction);
		}

		return (connection, transaction);
	}

	public void CommitAllTransactions()
	{
		foreach (var transaction in _globalTransactions)
			transaction.Value.Commit();
	}

	public void RollbackAllTransactions()
	{
		foreach (var transaction in _globalTransactions)
			transaction.Value.Rollback();
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposed)
		{
			if (disposing)
			{
				foreach (var connection in _conncetionsWithoutTransaction)
					connection.Value.Dispose();

				foreach (var connection in _conncetionsWithTransaction)
					connection.Value.Dispose();
			}

			disposed = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
