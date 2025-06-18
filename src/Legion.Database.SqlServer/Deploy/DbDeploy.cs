using Legion.Extensions;
using Microsoft.Extensions.Options;
using System.Text;

namespace Legion.Database.SqlServer.Deploy;

public class DbDeploy : IDisposable
{
	private readonly DbDeploySettings _dbDeploySettings;
	private readonly SqlConnectionManager _connectionManager;
	private readonly Action<string> _logText;
	private readonly Action<Exception> _logException;
	private bool disposed;

	public DbDeploy(
		IOptions<DbDeploySettings> dbDeploySettings,
		Action<string> logText,
		Action<Exception> logException)
	{
		Throw.IfArgumentNull(dbDeploySettings);
		Throw.IfArgumentNull(dbDeploySettings.Value);
		Throw.IfArgumentNull(logText);
		Throw.IfArgumentNull(logException);

		_dbDeploySettings = dbDeploySettings.Value;
		_connectionManager = new SqlConnectionManager(_dbDeploySettings);
		_logText = logText;
		_logException = logException;
	}

	public async Task<DbDeployResult> RunAllAsync(CancellationToken cancellationToken = default)
	{
		try
		{
			File.Delete(_dbDeploySettings.ErrorFilePath);
		}
		catch { }

		var mergedScriptSB = new StringBuilder();
		foreach (var sqlFileSettings in _dbDeploySettings.SqlFiles)
		{
			SqlFile sqlFile = null!;
			try
			{
				sqlFile = new SqlFile(false, _dbDeploySettings, sqlFileSettings, _connectionManager);

				_logText($"{GlobalContext.Instance.Now:yyyy-MM-dd HH:mm:ss} EXECUTE: {sqlFile.FilePath}");
				if (sqlFile.SqlFileExists())
				{
					await sqlFile.RunAsync(cancellationToken);

					if (!sqlFile.ExcludeFromMergeAllSqlFiles && _dbDeploySettings.MergeAllSqlFiles)
					{
						mergedScriptSB.AppendLine($"--{sqlFile.FilePath}");
						mergedScriptSB.AppendLine(sqlFile.SqlScript);
						mergedScriptSB.AppendLine();
						mergedScriptSB.AppendLine();
					}
				}
				else
				{
					Throw.InvalidOperationException(Exceptions.Internal.ErrorCodes.SqlFileException.FileNotExists, $"File path = {sqlFile.FilePath}");
				}
			}
			catch (Exception ex)
			{
				_logText($"{GlobalContext.Instance.Now:yyyy-MM-dd HH:mm:ss} run exception:");
				_logException(ex);
				return DbDeployResult.Failure(
					sqlFile?.FilePath,
					sqlFile?.ConnectionString,
					sqlFileSettings,
					ex.ToStringTrace());
			}
		}

		if (0 < _dbDeploySettings.DbPatch?.SqlFiles?.Count)
		{
			var patchManager = new PatchManager(_dbDeploySettings, _connectionManager);
			patchManager.Initialize();

			bool first = true;
			foreach (var patchSqlFileSettings in _dbDeploySettings.DbPatch.SqlFiles)
			{
				if (_dbDeploySettings.DbPatch.MergeAllSqlFiles)
				{
					mergedScriptSB.AppendLine("--PATCHES");
					mergedScriptSB.AppendLine();
				}

				first = false;
				SqlFile sqlFile = null!;
				try
				{
					sqlFile = new SqlFile(true, _dbDeploySettings, patchSqlFileSettings, _connectionManager);

					if (sqlFile.SqlFileExists())
					{
						_logText($"{GlobalContext.Instance.Now:yyyy-MM-dd HH:mm:ss} PATCH: {sqlFile.FilePath}");
						await patchManager.ExecutePatchCommands(sqlFile, cancellationToken);

						if (!sqlFile.ExcludeFromMergeAllSqlFiles && _dbDeploySettings.DbPatch.MergeAllSqlFiles)
						{
							mergedScriptSB.AppendLine($"--{sqlFile.FilePath}");
							mergedScriptSB.AppendLine(sqlFile.SqlScript);
							mergedScriptSB.AppendLine();
							mergedScriptSB.AppendLine();
						}
					}
					else
					{
						Throw.InvalidOperationException(Exceptions.Internal.ErrorCodes.SqlFileException.FileNotExists, $"File path = {sqlFile.FilePath}");
					}
				}
				catch (Exception ex)
				{
					_logText($"{GlobalContext.Instance.Now:yyyy-MM-dd HH:mm:ss} patch exception:");
					return DbDeployResult.Failure(
						sqlFile?.FilePath,
						sqlFile?.ConnectionString,
						patchSqlFileSettings,
						ex.ToStringTrace());
				}
			}
		}

		if (_dbDeploySettings.MergeAllSqlFiles
			|| _dbDeploySettings.DbPatch?.MergeAllSqlFiles == true
			|| 0 < mergedScriptSB.Length)
		{
			var mergedFileName = Path.Combine(_dbDeploySettings.WorkingDirectory, $"MERGED_SCRIPTS_{GlobalContext.Instance.Now:yyyy-MM-dd_HH-mm-ss}.sql");
			File.WriteAllText(mergedFileName, mergedScriptSB.ToString());
			_logText($"{GlobalContext.Instance.Now:yyyy-MM-dd HH:mm:ss} Created merged script file: {mergedFileName}");
		}

		_logText($"{GlobalContext.Instance.Now:yyyy-MM-dd HH:mm:ss} END");
		return DbDeployResult.Success();
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposed)
		{
			if (disposing)
			{
				_connectionManager.Dispose();
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
