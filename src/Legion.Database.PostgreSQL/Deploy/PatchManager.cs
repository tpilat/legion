using Legion.Cryptography;
using Legion.Extensions;
using Npgsql;
using System.Text;

namespace Legion.Database.PostgreSQL.Deploy;

public class PatchManager
{
	internal static class COMMANDS
	{
		public static readonly string previous_version = "--previous_version";
		public static readonly string set_version = "--set_version";
		public static readonly string set_permissions = "--set_permissions";
		public static readonly string condition = "--condition";
	}

	private const string COLUMN_PatchIdentifier = "\"PatchIdentifier\"";
	private const string COLUMN_Version = "\"Version\"";
	private const string COLUMN_Description = "\"Description\"";
	private const string COLUMN_Created = "\"Created\"";
	private const string COLUMN_FilePath = "\"FilePath\"";
	private const string COLUMN_Script = "\"Script\"";
	private const string COLUMN_Hash = "\"Hash\"";

	private readonly DbDeploySettings _dbDeploySettings;
	private readonly DbDeploySettings.DbPatchSettings _dbPatchSettings;

	private readonly Logger _logger;

	private readonly string[] _commands;

	private readonly string _dbPatchSchemaTable;
	private readonly SqlConnectionManager _sqlConnectionManager;
	private readonly NpgsqlConnection _npgsqlConnection;

	private bool _initialized = false;
	private DateTime? _lastPatchDateTime;
	private string? _lastVersion;

	public bool HasError => _logger.HasError;

	public PatchManager(DbDeploySettings dbDeploySettings, SqlConnectionManager sqlConnectionManager)
	{
		Throw.IfArgumentNull(dbDeploySettings);
		Throw.IfArgumentNull(dbDeploySettings.DbPatch);
		Throw.IfArgumentNull(sqlConnectionManager);

		_dbDeploySettings = dbDeploySettings;
		_dbPatchSettings = dbDeploySettings.DbPatch;
		_sqlConnectionManager = sqlConnectionManager;

		var connection = sqlConnectionManager.GetOrOpenConncection(_dbPatchSettings.PatchDatabaseName, null);
		_npgsqlConnection = connection.NpgsqlConnection;

		_dbPatchSchemaTable = $"{_dbPatchSettings.PatchSchemaName}.\"{_dbPatchSettings.PatchTableName}\"";

		_logger = new Logger();
		_commands = [ COMMANDS.previous_version, COMMANDS.set_version, COMMANDS.set_permissions, COMMANDS.condition ];
	}

	private void CreatePatchTable()
	{
		string? sql = null;
		try
		{
			var cmd = new NpgsqlCommand($"SELECT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_schema = '{_dbPatchSettings.PatchSchemaName}' AND table_name = '{_dbPatchSettings.PatchTableName}')", _npgsqlConnection);
			var existsObj = cmd.ExecuteScalar();

			if (existsObj is bool exists)
			{
				if (!exists)
				{
					_logger.LogNotice($"Creating table {_dbPatchSchemaTable} and grant usage to {_dbDeploySettings.TargetDbUsername}");

					sql = @$" 
SET client_min_messages TO WARNING; 

--DROP Schema If Exists {_dbPatchSettings.PatchSchemaName} Cascade;
--CREATE Schema {_dbPatchSettings.PatchSchemaName};

CREATE Schema IF NOT EXISTS {_dbPatchSettings.PatchSchemaName};

GRANT USAGE ON SCHEMA {_dbPatchSettings.PatchSchemaName} To {_dbDeploySettings.TargetDbUsername};

CREATE TABLE {_dbPatchSettings.PatchSchemaName}.""{_dbPatchSettings.PatchTableName}""
(
	{COLUMN_PatchIdentifier} timestamp without time zone NOT NULL,
	{COLUMN_Version} varchar(31) NOT NULL,
	{COLUMN_Description} text NULL,
	{COLUMN_Created} timestamp without time zone NOT NULL,
	{COLUMN_FilePath} varchar(255) NOT NULL,
	{COLUMN_Script} text NOT NULL,
	{COLUMN_Hash} varchar(127) NOT NULL
);

ALTER TABLE {_dbPatchSettings.PatchSchemaName}.""{_dbPatchSettings.PatchTableName}"" ADD CONSTRAINT ""PK_{_dbPatchSettings.PatchTableName}""
	PRIMARY KEY ({COLUMN_PatchIdentifier});

GRANT select, insert, update, delete On All Tables In Schema {_dbPatchSettings.PatchSchemaName} To {_dbDeploySettings.TargetDbUsername};
GRANT usage On All Sequences In Schema {_dbPatchSettings.PatchSchemaName} To {_dbDeploySettings.TargetDbUsername};
";

					cmd = new NpgsqlCommand(sql, _npgsqlConnection);
					cmd.ExecuteNonQuery();

					_logger.LogSuccess($"Created table {_dbPatchSchemaTable} and granted usage to {_dbDeploySettings.TargetDbUsername}");
				}
			}
			else
			{
				Throw.InvalidOperationException(Exceptions.Internal.ErrorCodes.PatchException.ReadFromPatchTable, $"Invalid {nameof(existsObj)} = {existsObj}");
			}
		}
		catch (Exception ex)
		{
			if (!string.IsNullOrWhiteSpace(sql))
			{
				_logger.LogError($"{nameof(PatchManager)}.{nameof(CreatePatchTable)}: SCRIPT:");
				_logger.LogInfo(sql!);
			}

			_logger.LogError($"{nameof(PatchManager)}.{nameof(CreatePatchTable)}", ex);
			throw;
		}
	}

	private DateTime GetLastPatch()
	{
		var defaultPatchDate = _dbPatchSettings.FromDateTime ?? DateTime.MinValue;
		var result = defaultPatchDate;

		try
		{
			var cmd = new NpgsqlCommand($"SELECT {COLUMN_PatchIdentifier} FROM {_dbPatchSchemaTable} ORDER BY {COLUMN_PatchIdentifier} DESC LIMIT 1", _npgsqlConnection);
			using var reader = cmd.ExecuteReader();
			if (reader.Read())
				result = reader.GetNullableDateTime(0) ?? defaultPatchDate;
			else
				result = defaultPatchDate;
		}
		catch (Exception ex)
		{
			_logger.LogError($"{nameof(PatchManager)}.{nameof(GetLastPatch)}", ex);
			throw;
		}

		//max
		return defaultPatchDate <= result
			? result
			: defaultPatchDate;
	}

	private string GetLastVersion(DateTime patchDate)
	{
		var defaultVersion = string.IsNullOrWhiteSpace(_dbPatchSettings.PreviousVersion)
			? new Version("0.0.0.0")
			: new Version(_dbPatchSettings.PreviousVersion);

		var result = "";
		try
		{
			var cmd = new NpgsqlCommand($"SELECT {COLUMN_Version} FROM {_dbPatchSchemaTable} WHERE {COLUMN_PatchIdentifier} = @patchDate", _npgsqlConnection);
			cmd.Parameters.AddWithValue("patchDate", (object)patchDate ?? DBNull.Value);

			using var reader = cmd.ExecuteReader();
			if (reader.Read())
				result = reader.GetString(0);
		}
		catch (Exception ex)
		{
			_logger.LogError($"{nameof(PatchManager)}.{nameof(GetLastVersion)}", ex);
			throw;
		}

		if (string.IsNullOrWhiteSpace(result))
		{
			return defaultVersion.ToString();
		}
		else
		{
			var resultVersion = new Version(result);

			//max
			return defaultVersion <= resultVersion
				? result
				: defaultVersion.ToString();
		}
	}

	private readonly object _initLock = new();
	public void Initialize()
	{
		if (_initialized)
			return;

		lock (_initLock)
		{
			if (_initialized)
				return;

			_initialized = true;

			CreatePatchTable();
			_lastPatchDateTime = GetLastPatch();
			_lastVersion = GetLastVersion(_lastPatchDateTime.Value);
		}
	}

	private DateTime? GetPatchFileDateTime(string filePath)
	{
		if (!_initialized)
			Throw.InvalidOperationException(Exceptions.Internal.ErrorCodes.PatchException.NotInitialized);
	
		Throw.IfArgumentNullOrWhiteSpace(filePath);

		var fileName = Path.GetFileNameWithoutExtension(filePath);
		var fileNameSplit = fileName.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);

		try
		{
			if (!DateTime.TryParse(fileNameSplit[0].Trim(), out DateTime dateTime))
				Throw.InvalidOperationException(Exceptions.Internal.ErrorCodes.PatchException.InvalidPatchFileName, $"Invalid fileName.DateTime in {filePath}");

			dateTime = dateTime.Date;
			if (1 < fileNameSplit.Length)
			{
				var sec = fileNameSplit[1].Trim();
				if (!sec.StartsWith("#"))
				{
					while (sec.StartsWith("0"))
						sec = sec.TrimPrefix("0");

					if (int.TryParse(sec, out int seconds))
					{
						if (seconds <= 0)
							Throw.InvalidOperationException(Exceptions.Internal.ErrorCodes.PatchException.InvalidPatchFileName, $"Invalid fileName.Seconds == 0 in {filePath}");
						if (86399 < seconds)
							Throw.InvalidOperationException(Exceptions.Internal.ErrorCodes.PatchException.InvalidPatchFileName, $"Invalid fileName.Seconds > 86399 in {filePath}");

						dateTime = dateTime.AddSeconds(seconds);
					}
					else
						Throw.InvalidOperationException(Exceptions.Internal.ErrorCodes.PatchException.InvalidPatchFileName, $"Invalid fileName.Seconds in {filePath}");
				}
			}

			if (_lastPatchDateTime < dateTime)
				return dateTime;

			MoveFileToArchive(filePath);
			return null;
		}
		catch (Exception ex)
		{
			_logger.LogError($"{nameof(PatchManager)}.{nameof(GetPatchFileDateTime)}", ex);
			throw;
		}
	}

	private void MoveFileToArchive(string filePath)
	{
		if (!_dbPatchSettings.Archive)
			return;

		try
		{
			var fileName = Path.GetFileName(filePath);
			var dir = Path.GetDirectoryName(filePath);
			dir = Path.Combine(dir!, "Archive");
			if (!Directory.Exists(dir))
				Directory.CreateDirectory(dir);

			var newFilePath = Path.Combine(dir, fileName);
			File.Move(filePath, newFilePath);

			_logger.LogInfo($"Archived file to {newFilePath}");
		}
		catch (Exception ex)
		{
			_logger.LogError($"{nameof(PatchManager)}.{nameof(MoveFileToArchive)}", ex);
		}
	}

	public async Task ExecutePatchCommands(SqlFile sqlFile, CancellationToken cancellationToken = default)
	{
		if (!_initialized)
			Throw.InvalidOperationException(Exceptions.Internal.ErrorCodes.PatchException.NotInitialized);

		Throw.IfArgumentNull(sqlFile);

		var patchDate = GetPatchFileDateTime(sqlFile.FilePath);
		if (!patchDate.HasValue)
			return;

		var version = _lastVersion!;
		_logger.LogNewLine();
		if (_logger.HasError)
		{
			_logger.LogWarning($"SKIP {sqlFile.FilePath}");
			return;
		}
		try
		{
			_logger.LogNotice($"Executing {sqlFile}");

			var sqlLines = File.ReadAllLines(sqlFile.FilePath, sqlFile.Encoding);

			var previous_versions = new List<string>();
			var allowWildcardPreviousVersions = false;

			//COMMANDS
			foreach (var command in _commands)
			{
				if (0 < sqlLines.Length)
				{
					var line = sqlLines[0].Trim();

					if (command == COMMANDS.previous_version && line.StartsWith(command))
					{
						line = line.TrimPrefix(command).Trim();
						allowWildcardPreviousVersions = line == "*";
						if (!allowWildcardPreviousVersions)
						{
							var split = line.Split([';'], StringSplitOptions.RemoveEmptyEntries);
							previous_versions = split.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
						}
					}
					else if (command == COMMANDS.set_version && line.StartsWith(command))
					{
						line = line.TrimPrefix(command).Trim();
						if (!string.IsNullOrWhiteSpace(line))
							version = line;
					}
					else if (command == COMMANDS.set_permissions && line.StartsWith(command))
					{
						//setPermissions = true;
					}
					else if (command == COMMANDS.condition && line.StartsWith(command))
					{
						var conditionalExecuting = line.TrimPrefix(command).Trim();
						if (!string.IsNullOrWhiteSpace(conditionalExecuting))
						{
							if (_dbPatchSettings.AllowedConditions != null && 0 < _dbPatchSettings.AllowedConditions.Count && !_dbPatchSettings.AllowedConditions.Contains(conditionalExecuting))
							{
								_logger.LogWarning($"SKIPPED patch: {sqlFile.FilePath} - MISSING CONDITION {conditionalExecuting}");
								_lastVersion = version;
								return;
							}
						}
					}
					else
					{
						continue; //command sa nenasiel
					}

					sqlLines = sqlLines.Skip(1).ToArray();
				}
			}

			if (!allowWildcardPreviousVersions)
			{
				if (previous_versions.Count == 0)
					throw new Exception($"Missing COMMAND {COMMANDS.previous_version} | hint: {nameof(_lastVersion)} = {_lastVersion}");

				if (!previous_versions.Contains(_lastVersion!))
					throw new Exception($"Incorrect previous versions [{string.Join(";", previous_versions)}] ... Required version is {_lastVersion}");
			}

			var sbDescription = new StringBuilder();
			var fileName = Path.GetFileNameWithoutExtension(sqlFile.FilePath);
			var noteIndex = fileName.IndexOf("#");
			if (-1 < noteIndex && noteIndex < fileName.Length - 1)
				sbDescription.AppendLine(fileName.SubstringSafe(noteIndex));

			foreach (var sqlLine in sqlLines)
			{
				if (sqlLine.StartsWith("--"))
					sbDescription.AppendLine(sqlLine.TrimPrefix("--"));
				else
					break;
			}

			string? description = sbDescription.ToString();
			if (string.IsNullOrWhiteSpace(description))
				description = null;

			using var cmd1 = new NpgsqlCommand(sqlFile.SqlScript!);
			await sqlFile.ExecuteNonQueryAsync(cmd1, false, cancellationToken);

			var sqlBytes = sqlFile.Encoding.GetBytes(sqlFile.SqlScript!);
			var hash = HashHelper.ComputeSha256Hash(sqlBytes);

			var cmd2 = new NpgsqlCommand($"INSERT INTO {_dbPatchSchemaTable} ({COLUMN_PatchIdentifier}, {COLUMN_Version}, {COLUMN_Description}, {COLUMN_Created}, {COLUMN_FilePath}, {COLUMN_Script}, {COLUMN_Hash}) VALUES (@patchIdentifier, @ver, @description, @created, @filePath, @script, @hash)");
			cmd2.Parameters.AddWithValue("patchIdentifier", (object)patchDate ?? DBNull.Value);
			cmd2.Parameters.AddWithValue("ver", version);
			cmd2.Parameters.AddWithValue("description", (object?)description ?? DBNull.Value);
			cmd2.Parameters.AddWithValue("created", GlobalContext.Instance.Now);
			cmd2.Parameters.AddWithValue("filePath", sqlFile.FilePath);
			cmd2.Parameters.AddWithValue("script", (object)sqlFile.SqlScript! ?? DBNull.Value);
			cmd2.Parameters.AddWithValue("hash", (object)hash ?? DBNull.Value);

			var result = (long)await sqlFile.ExecuteNonQueryAsync(cmd2, true, cancellationToken);

			if (result == 0)
				throw new InvalidOperationException($"No patch was inserted into {_dbPatchSchemaTable}");

			MoveFileToArchive(sqlFile.FilePath);

			_logger.LogSuccess($"Successfully patched: {sqlFile.FilePath}");
		}
		catch (Exception ex)
		{
			_logger.LogError($"{nameof(PatchManager)}.{nameof(ExecutePatchCommands)}({nameof(sqlFile.FilePath)} = {sqlFile.FilePath})", ex);
			throw;
		}

		_lastVersion = version;
	}
	public string? GetLog()
	{
		var log = _logger.ToString();
		return string.IsNullOrWhiteSpace(log)
			? null
			: log;
	}
}
