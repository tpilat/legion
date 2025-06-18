using Legion.Extensions;
using Legion.IOUtils;
using Npgsql;
using System.Text;

namespace Legion.Database.PostgreSQL.Deploy;

public class SqlFile
{
	private const string TargetDatabase = "#TargetDatabase#";
	private const string CurrentDatabase = "#CurrentDatabase#";
	private const string AdminUser = "#AdminUser#";
	private const string TargetDbUsername = "#TargetDbUsername#";
	private const string TargetDbPassword = "#TargetDbPassword#";

	private readonly bool _isPatchFile;
	private readonly DbDeploySettings _dbDeploySettings;
	private readonly DbDeploySettings.SqlFileSettings _sqlFileSettings;
	private readonly SqlConnectionManager _connectionManager;

	public string FilePath { get; }
	public Encoding Encoding => _sqlFileSettings.Encoding;
	public string? ConnectionString { get; private set; }
	public string? SqlScript { get; private set; }
	public bool ExcludeFromMergeAllSqlFiles { get; set; }

	public SqlFile(
		bool isPatchFile,
		DbDeploySettings dbDeploySettings,
		DbDeploySettings.SqlFileSettings sqlFileSettings,
		SqlConnectionManager connectionManager)
	{
		Throw.IfArgumentNull(dbDeploySettings);
		Throw.IfArgumentNull(sqlFileSettings);
		Throw.IfArgumentNull(connectionManager);

		_isPatchFile = isPatchFile;
		_dbDeploySettings = dbDeploySettings;
		_sqlFileSettings = sqlFileSettings;
		_connectionManager = connectionManager;

		var workingDirectory = _dbDeploySettings.WorkingDirectory;

		FilePath = DirectoryHelper.CombinePaths(workingDirectory, _sqlFileSettings.FilePath, true);
	}

	public bool SqlFileExists()
		=> File.Exists(FilePath);

	public void LoadScriptAndReplace()
	{
		var originalScript = File.ReadAllText(FilePath, _sqlFileSettings.Encoding);

		var script = originalScript
			.Replace(TargetDatabase, _dbDeploySettings.TargetDatabase)
			.Replace(CurrentDatabase, _sqlFileSettings.CurrentDatabase)
			.Replace(AdminUser, _dbDeploySettings.AdminUser)
			.Replace(TargetDbUsername, _dbDeploySettings.TargetDbUsername)
			.Replace(TargetDbPassword, _dbDeploySettings.TargetDbPassword);

		if (!string.IsNullOrWhiteSpace(_sqlFileSettings.FilledScriptPostfix) && script != originalScript)
		{
			var directory = Path.GetDirectoryName(FilePath);
			var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(FilePath);
			var extension = Path.GetExtension(FilePath);

			var filledFilePath = Path.Combine(directory!, $"{fileNameWithoutExtension}{_sqlFileSettings.FilledScriptPostfix}{extension}");
			File.WriteAllText(filledFilePath, script, _sqlFileSettings.Encoding);
		}

		SqlScript = script;
	}

	private NpgsqlConnection? _npgsqlConnection;
	private NpgsqlTransaction? _npgsqlTransaction;

	public async Task<int> ExecuteNonQueryAsync(NpgsqlCommand npgsqlCommand, bool commitAndDisposeLocalTransactions, CancellationToken cancellationToken = default)
	{
		Throw.IfArgumentNull(npgsqlCommand);

		if (_npgsqlConnection == null)
		{
			var connections = _connectionManager.GetOrOpenConncection(_sqlFileSettings.CurrentDatabase, _sqlFileSettings.Transaction);
			_npgsqlConnection = connections.NpgsqlConnection;
			_npgsqlTransaction = connections.NpgsqlTransaction;
		}

		ConnectionString = _npgsqlConnection.ConnectionString;

		if (_sqlFileSettings.Transaction == DbDeploySettings.TransactionMode.Local && _npgsqlTransaction == null)
			Throw.InvalidOperationException(Exceptions.Internal.ErrorCodes.TransactionException.NoTransaction);

		try
		{
			npgsqlCommand.Connection = _npgsqlConnection;

			if (_npgsqlTransaction != null)
				npgsqlCommand.Transaction = _npgsqlTransaction;

			if (_sqlFileSettings.CommandTimeout.HasValue)
				npgsqlCommand.CommandTimeout = _sqlFileSettings.CommandTimeout.Value;

			var affectedRows = await npgsqlCommand.ExecuteNonQueryAsync(cancellationToken);

			if (commitAndDisposeLocalTransactions && _sqlFileSettings.Transaction == DbDeploySettings.TransactionMode.Local)
			{
				if (_npgsqlTransaction != null)
					await _npgsqlTransaction.CommitAsync(cancellationToken);

				_npgsqlConnection.Dispose();
				_npgsqlConnection = null;
			}

			return affectedRows;
		}
		catch
		{
			if (_sqlFileSettings.Transaction == DbDeploySettings.TransactionMode.Local)
			{
				try
				{
					if (_npgsqlTransaction != null)
						await _npgsqlTransaction.RollbackAsync(cancellationToken);

					_npgsqlConnection?.Dispose();
					_npgsqlConnection = null;
				}
				catch { }
			}

			throw;
		}
	}

	public async Task RunAsync(CancellationToken cancellationToken = default)
	{
		LoadScriptAndReplace();
		using var cmd = new NpgsqlCommand(SqlScript!);
		await ExecuteNonQueryAsync(cmd, true, cancellationToken);
	}

	public override string ToString()
		=> FilePath;
}
