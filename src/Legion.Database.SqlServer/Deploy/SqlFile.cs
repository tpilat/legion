using Legion.IOUtils;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;

namespace Legion.Database.SqlServer.Deploy;

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

	private SqlConnection? _sqlServerConnection;
	private SqlTransaction? _sqlServerTransaction;

	public async Task<int> ExecuteNonQueryAsync(SqlCommand sqlServerCommand, bool commitAndDisposeLocalTransactions, CancellationToken cancellationToken = default)
	{
		Throw.IfArgumentNull(sqlServerCommand);

		if (_sqlServerConnection == null)
		{
			var connections = _connectionManager.GetOrOpenConncection(_sqlFileSettings.CurrentDatabase, _sqlFileSettings.Transaction);
			_sqlServerConnection = connections.SqlConnection;
			_sqlServerTransaction = connections.SqlTransaction;
		}

		ConnectionString = _sqlServerConnection.ConnectionString;

		if (_sqlFileSettings.Transaction == DbDeploySettings.TransactionMode.Local && _sqlServerTransaction == null)
			Throw.InvalidOperationException(Exceptions.Internal.ErrorCodes.TransactionException.NoTransaction);

		try
		{
			sqlServerCommand.Connection = _sqlServerConnection;

			if (_sqlServerTransaction != null)
				sqlServerCommand.Transaction = _sqlServerTransaction;

			if (_sqlFileSettings.CommandTimeout.HasValue)
				sqlServerCommand.CommandTimeout = _sqlFileSettings.CommandTimeout.Value;

			var affectedRows = await sqlServerCommand.ExecuteNonQueryAsync(cancellationToken);

			if (commitAndDisposeLocalTransactions && _sqlFileSettings.Transaction == DbDeploySettings.TransactionMode.Local)
			{
				if (_sqlServerTransaction != null)
					await _sqlServerTransaction.CommitAsync(cancellationToken);

				_sqlServerConnection.Dispose();
				_sqlServerConnection = null;
			}

			return affectedRows;
		}
		catch
		{
			if (_sqlFileSettings.Transaction == DbDeploySettings.TransactionMode.Local)
			{
				try
				{
					if (_sqlServerTransaction != null)
						await _sqlServerTransaction.RollbackAsync(cancellationToken);

					_sqlServerConnection?.Dispose();
					_sqlServerConnection = null;
				}
				catch { }
			}

			throw;
		}
	}

	public async Task RunAsync(CancellationToken cancellationToken = default)
	{
		LoadScriptAndReplace();
		var scripts = SplitSqlStatements(SqlScript!);
		foreach (var script in scripts)
		{
			using var cmd = new SqlCommand(script);
			await ExecuteNonQueryAsync(cmd, true, cancellationToken);
		}
	}

	public static IEnumerable<string> SplitSqlStatements(string sqlScript)
	{
		sqlScript = Regex.Replace(sqlScript, @"(\r\n|\n\r|\n|\r)", "\n");

		var statements = Regex.Split(
				sqlScript,
				@"^\s*GO\s*\d*\s*(?:--.*)?$",
				RegexOptions.Multiline |
				RegexOptions.IgnorePatternWhitespace |
				RegexOptions.IgnoreCase);

		return statements
			.Where(x => !string.IsNullOrWhiteSpace(x))
			.Select(x => x.Trim(' ', '\n'));
	}

	public override string ToString()
		=> FilePath;
}
