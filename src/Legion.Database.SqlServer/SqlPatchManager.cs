using Legion.Cryptography;
using Legion.Extensions;
using Microsoft.Data.SqlClient;
using System.Text;

namespace Legion.Database.SqlServer;

public class SqlPatchManager
{
	private static class COMMANDS
	{
		public static readonly string previous_version = "--previous_version";
		public static readonly string set_version = "--set_version";
		public static readonly string set_permissions = "--set_permissions";
		public static readonly string condition = "--condition";
	}

	private const string _DBPATCH_SCHEMA = "dsg";
	private const string _DBPATCH_TABLE = "DBPatch";

	private const string COLUMN_PatchIdentifier = "\"PatchIdentifier\"";
	private const string COLUMN_Version = "\"Version\"";
	private const string COLUMN_Description = "\"Description\"";
	private const string COLUMN_Created = "\"Created\"";
	private const string COLUMN_FilePath = "\"FilePath\"";
	private const string COLUMN_Script = "\"Script\"";
	private const string COLUMN_Hash = "\"Hash\"";

	private readonly string[] _commands;

	private readonly string _dbPatchSchema;
	private readonly string _dbPatchTable;
	private readonly string _dbPatchSchemaTable;
	private readonly Encoding _sqlFilesEncoding;
	private readonly string _databaseName;
	private readonly string _grantedUser;
	private readonly bool _archive;
	private readonly IReadOnlyList<string>? _allowedConditions;
	private readonly DateTime? _defaultLastPatchDateTime;
	private readonly string? _firstPreviousVersion;

	private string _connectionString;
	private SqlConnection? _connection;

	private bool? _succeeded;
	private StringBuilder _logger;

	public SqlPatchManager(
		SqlConnection sqlServerConnection,
		string sqlUserName,
		string databaseName,
		bool archive,
		IReadOnlyList<string>? allowedConditions,
		DateTime? defaultLastPatchDateTime = null,
		string? firstPreviousVersion = null,
		string? dbSchemaName = null,
		string? dbTableName = null)
	{
		Throw.IfArgumentNull(sqlServerConnection);

		_dbPatchSchema = string.IsNullOrWhiteSpace(dbSchemaName)
			? _DBPATCH_TABLE
			: dbSchemaName!;

		_dbPatchTable = string.IsNullOrWhiteSpace(dbTableName)
			? _DBPATCH_SCHEMA
			: dbTableName!;

		_dbPatchSchemaTable = _dbPatchSchema + ".\"" + _dbPatchTable + "\""; ;

		_connectionString = null!;
		_connection = sqlServerConnection;
		_grantedUser = sqlUserName;
		_databaseName = databaseName;
		_sqlFilesEncoding = Encoding.UTF8;
		_defaultLastPatchDateTime = defaultLastPatchDateTime;
		_firstPreviousVersion = firstPreviousVersion;
		_archive = archive;
		_allowedConditions = allowedConditions;

		_commands = [COMMANDS.previous_version, COMMANDS.set_version, COMMANDS.set_permissions, COMMANDS.condition];

		_logger = new();
		_succeeded = false;
	}

	public SqlPatchManager(
		string connectionString,
		string sqlUserName,
		string databaseName,
		bool archive,
		IReadOnlyList<string>? allowedConditions,
		DateTime? defaultLastPatchDateTime = null,
		string? firstPreviousVersion = null,
		string? dbSchemaName = null,
		string? dbTableName = null)
	{
		_dbPatchSchema = string.IsNullOrWhiteSpace(dbSchemaName)
			? _DBPATCH_TABLE
			: dbSchemaName!;

		_dbPatchTable = string.IsNullOrWhiteSpace(dbTableName)
			? _DBPATCH_SCHEMA
			: dbTableName!;

		_dbPatchSchemaTable = _dbPatchSchema + ".\"" + _dbPatchTable + "\""; ;

		_connectionString = connectionString;
		_grantedUser = sqlUserName;
		_databaseName = databaseName;
		_sqlFilesEncoding = Encoding.UTF8;
		_defaultLastPatchDateTime = defaultLastPatchDateTime;
		_firstPreviousVersion = firstPreviousVersion;
		_archive = archive;
		_allowedConditions = allowedConditions;

		_commands = [COMMANDS.previous_version, COMMANDS.set_version, COMMANDS.set_permissions, COMMANDS.condition];

		_logger = new();
		_succeeded = false;
	}

	public bool PatchDirectory(
		string patchDirectoryFullPath,
		out string? log)
	{
		_logger = new();
		_succeeded = false;

		if (!Directory.Exists(patchDirectoryFullPath))
		{
			log = $"Directory {patchDirectoryFullPath} does not exists.";
			_succeeded = false;
			return _succeeded.Value;
		}

		try
		{
			ConnectToDB();
			CreatePatchTable();
			var lastPatchDateTime = GetLastPatch();
			var lastVersion = GetLastVersion(lastPatchDateTime);

			_logger.AppendLine($"DB: {nameof(lastPatchDateTime)} = {lastPatchDateTime:dd.MM.yyyy HH:mm:ss}");
			_logger.AppendLine($"DB: {nameof(lastVersion)} = {lastVersion}");

			var files = LoadFiles(patchDirectoryFullPath);
			if (files != null)
			{
				var newPatchFiles = FilesToPatchDate(files, lastPatchDateTime);
				var patchDates = newPatchFiles.Keys.OrderBy(x => x).ToList();
				foreach (var patchDate in patchDates)
					lastVersion = Patch(patchDate, lastVersion, newPatchFiles[patchDate], true);
			}

			if (!_succeeded.HasValue)
				_succeeded = true;
		}
		catch (Exception ex)
		{
			_logger.AppendLine(ex.ToStringTrace());
			_succeeded = false;
		}

		var resultLog = _logger.ToString();
		log = string.IsNullOrWhiteSpace(resultLog)
			? null
			: resultLog;

		return _succeeded.Value;
	}

	public bool PatchScript(
		string patchScript,
		out string? log)
	{
		_logger = new();
		_succeeded = false;

		if (string.IsNullOrWhiteSpace(patchScript))
		{
			log = $"{nameof(patchScript)} == null";
			_succeeded = false;
			return _succeeded.Value;
		}

		try
		{
			ConnectToDB();
			CreatePatchTable();
			var lastPatchDateTime = GetLastPatch();
			var lastVersion = GetLastVersion(lastPatchDateTime);

			_logger.AppendLine($"DB: {nameof(lastPatchDateTime)} = {lastPatchDateTime:dd.MM.yyyy HH:mm:ss}");
			_logger.AppendLine($"DB: {nameof(lastVersion)} = {lastVersion}");

			var newPatchFiles = FilesToPatchDate([patchScript], lastPatchDateTime);
			var patchDates = newPatchFiles.Keys.OrderBy(x => x).ToList();
			foreach (var patchDate in patchDates)
				lastVersion = Patch(patchDate, lastVersion, newPatchFiles[patchDate], false);

			if (!_succeeded.HasValue)
				_succeeded = true;
		}
		catch (Exception ex)
		{
			_logger.AppendLine(ex.ToStringTrace());
			_succeeded = false;
		}

		var resultLog = _logger.ToString();
		log = string.IsNullOrWhiteSpace(resultLog)
			? null
			: resultLog;

		return _succeeded.Value;
	}

	private bool ConnectToDB()
	{
		if (_connection != null)
			return true;

		try
		{
			if (string.IsNullOrWhiteSpace(_connectionString))
				throw new InvalidOperationException($"No {nameof(_connectionString)} defined.");

			if (_connectionString.Contains("{0}"))
				_connectionString = string.Format(_connectionString, System.Environment.GetEnvironmentVariable("MSSQLPASSWORD"));

			_logger.AppendLine($"Connecting to {_connectionString}");

			_connection = new SqlConnection(_connectionString);
			_connection.Open();

			_logger.AppendLine($"Connected to {_connectionString}");

			return true;
		}
		catch (Exception ex)
		{
			_logger.AppendLine($"Connecting to {_connectionString}");
			_logger.AppendLine(ex.ToStringTrace());
			_succeeded = false;
			throw;
		}
	}

	private void CreatePatchTable()
	{
		string? sql = null;
		try
		{
			var cmd = new SqlCommand($"SELECT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_schema = '{_dbPatchSchema}' AND table_name = '{_dbPatchTable}')", _connection);
			var existsObj = cmd.ExecuteScalar();

			if (existsObj is bool exists)
			{
				if (!exists)
				{
					_logger.AppendLine($"Creating table {_dbPatchSchemaTable} and grant usage to {_grantedUser}");

					sql = @$" 
SET client_min_messages TO WARNING; 

--DROP Schema If Exists {_dbPatchSchema} Cascade;
--CREATE Schema {_dbPatchSchema};

CREATE Schema IF NOT EXISTS {_dbPatchSchema};

GRANT USAGE ON SCHEMA {_dbPatchSchema} To {_grantedUser};

CREATE TABLE {_dbPatchSchema}.""{_dbPatchTable}""
(
	{COLUMN_PatchIdentifier} timestamp without time zone NOT NULL,
	{COLUMN_Version} varchar(31) NOT NULL,
	{COLUMN_Description} text NULL,
	{COLUMN_Created} timestamp without time zone NOT NULL,
	{COLUMN_FilePath} varchar(255) NOT NULL,
	{COLUMN_Script} text NOT NULL,
	{COLUMN_Hash} varchar(127) NOT NULL
);

ALTER TABLE {_dbPatchSchema}.""{_dbPatchTable}"" ADD CONSTRAINT ""PK_{_dbPatchTable}""
	PRIMARY KEY ({COLUMN_PatchIdentifier});

GRANT select, insert, update, delete On All Tables In Schema {_dbPatchSchema} To {_grantedUser};
GRANT usage On All Sequences In Schema {_dbPatchSchema} To {_grantedUser};
";

					cmd = new SqlCommand(sql, _connection);
					cmd.ExecuteNonQuery();

					_logger.AppendLine($"Created table {_dbPatchSchemaTable} and granted usage to {_grantedUser}");
				}
			}
			else
			{
				throw new InvalidOperationException($"Invalid {nameof(existsObj)} = {existsObj}");
			}
		}
		catch (Exception ex)
		{
			if (!string.IsNullOrWhiteSpace(sql))
			{
				_logger.AppendLine($"{nameof(SqlPatchManager)}.{nameof(CreatePatchTable)}: SCRIPT:");
				_logger.AppendLine(sql);
			}

			_logger.AppendLine($"{nameof(SqlPatchManager)}.{nameof(CreatePatchTable)}");
			_logger.AppendLine(ex.ToStringTrace());
			_succeeded = false;
			throw;
		}
	}

	private DateTime GetLastPatch()
	{
		var defaultPatchDate = _defaultLastPatchDateTime ?? DateTime.MinValue;
		var result = defaultPatchDate;

		try
		{
			var cmd = new SqlCommand($"SELECT {COLUMN_PatchIdentifier} FROM {_dbPatchSchemaTable} ORDER BY {COLUMN_PatchIdentifier} DESC LIMIT 1", _connection);
			using var reader = cmd.ExecuteReader();
			if (reader.Read())
				result = reader.GetNullableDateTime(0) ?? defaultPatchDate;
			else
				result = defaultPatchDate;
		}
		catch (Exception ex)
		{
			_logger.AppendLine($"{nameof(SqlPatchManager)}.{nameof(GetLastPatch)}");
			_logger.AppendLine(ex.ToStringTrace());
			_succeeded = false;
			throw;
		}

		//max
		return defaultPatchDate <= result
			? result
			: defaultPatchDate;
	}

	private string GetLastVersion(DateTime patchDate)
	{
		var defaultVersion = string.IsNullOrWhiteSpace(_firstPreviousVersion)
			? new Version("0.0.0.0")
			: new Version(_firstPreviousVersion);

		var result = "";
		try
		{
			var cmd = new SqlCommand($"SELECT {COLUMN_Version} FROM {_dbPatchSchemaTable} WHERE {COLUMN_PatchIdentifier} = @patchDate", _connection);
			cmd.Parameters.AddWithValue("patchDate", (object)patchDate ?? DBNull.Value);

			using var reader = cmd.ExecuteReader();
			if (reader.Read())
				result = reader.GetString(0);
		}
		catch (Exception ex)
		{
			_logger.AppendLine($"{nameof(SqlPatchManager)}.{nameof(GetLastVersion)}");
			_logger.AppendLine(ex.ToStringTrace());
			_succeeded = false;
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

	private string[]? LoadFiles(string patchDirectoryFullPath)
	{
		string[]? result;

		try
		{
			if (string.IsNullOrWhiteSpace(patchDirectoryFullPath))
				throw new InvalidOperationException($"No {nameof(patchDirectoryFullPath)} defined.");

			if (!Directory.Exists(patchDirectoryFullPath))
				throw new InvalidOperationException($"Directory {patchDirectoryFullPath} does not exists.");

			_logger.AppendLine($"Getting files from {patchDirectoryFullPath}");

			result = Directory.GetFiles(patchDirectoryFullPath, "*.sql", SearchOption.TopDirectoryOnly);

			if (result == null || result.Length == 0)
			{
				_logger.AppendLine($"No sql files found in {patchDirectoryFullPath}");
				return null;
			}

			_logger.AppendLine($"Found {result.Length} sql files in {patchDirectoryFullPath}");
			return result;
		}
		catch (Exception ex)
		{
			_logger.AppendLine($"Loading files from {patchDirectoryFullPath}");
			_logger.AppendLine(ex.ToStringTrace());
			_succeeded = false;
			throw;
		}
	}

	private Dictionary<DateTime, string> FilesToPatchDate(string[] files, DateTime lastPatchDateTime)
	{
		var result = new Dictionary<DateTime, string>();

		try
		{
			foreach (var file in files)
			{
				var fileName = Path.GetFileNameWithoutExtension(file);
				var fileNameSplit = fileName.Split(['_'], StringSplitOptions.RemoveEmptyEntries);

				if (DateTime.TryParse(fileNameSplit[0].Trim(), out DateTime dateTime))
				{
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
									throw new InvalidOperationException($"Invalid fileName.Seconds == 0 in {file}");
								if (86399 < seconds)
									throw new InvalidOperationException($"Invalid fileName.Seconds > 86399 in {file}");

								dateTime = dateTime.AddSeconds(seconds);
							}
							else
								throw new InvalidOperationException($"Invalid fileName.Seconds in {file}");
						}
					}

					if (lastPatchDateTime < dateTime)
					{
						if (!result.TryAdd(dateTime, file))
							throw new InvalidOperationException($"Multiple fileName.DateTime in {file}"); //moze to nastat ked jeden subor sa vola 2021-11-17_1 a dalsi sa vola 2021-11-17_01, 2021-11-17_001, 2021-11-17_001, ...
					}
					else
						MoveFileToArchive(file);
				}
				else
				{
					throw new InvalidOperationException($"Invalid fileName.DateTime in {file}");
				}
			}
		}
		catch (Exception ex)
		{
			_logger.AppendLine($"{nameof(SqlPatchManager)}.{nameof(FilesToPatchDate)}");
			_logger.AppendLine(ex.ToStringTrace());
			_succeeded = false;
			throw;
		}

		return result;
	}

	private string Patch(DateTime patchDate, string lastVersion, string file, bool loadFromFile)
	{
		var version = lastVersion;

		if (_succeeded == false)
		{
			_logger.AppendLine($"SKIP {file}");
			return version;
		}
		try
		{
			if (loadFromFile)
				_logger.AppendLine($"Executing {file}");

			Throw.IfArgumentNullOrWhiteSpace(file);

			string sql;
			string[] sqlLines;

			if (loadFromFile)
			{
				sql = File.ReadAllText(file, _sqlFilesEncoding);
				sqlLines = File.ReadAllLines(file, _sqlFilesEncoding);
			}
			else
			{
				sql = file;
				sqlLines = file.Split([Environment.NewLine], StringSplitOptions.None);
			}

			var sqlBytes = _sqlFilesEncoding.GetBytes(sql);
			var hash = HashHelper.ComputeSha256Hash(sqlBytes);

			var previous_versions = new List<string>();
			var allowWildcardPreviousVersions = false;
			var setPermissions = false;

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
						setPermissions = true;
					}
					else if (command == COMMANDS.condition && line.StartsWith(command))
					{
						var conditionalExecuting = line.TrimPrefix(command).Trim();
						if (!string.IsNullOrWhiteSpace(conditionalExecuting))
						{
							if (_allowedConditions != null && 0 < _allowedConditions.Count && !_allowedConditions.Contains(conditionalExecuting))
							{
								_logger.AppendLine($"SKIPPED patch: {file} - MISSING CONDITION {conditionalExecuting}");
								return version;
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
					throw new Exception($"Missing COMMAND {COMMANDS.previous_version} | hint: {nameof(lastVersion)} = {lastVersion}");

				if (!previous_versions.Contains(lastVersion))
					throw new Exception($"Incorrect previous versions [{string.Join(";", previous_versions)}] ... Required version is {lastVersion}");
			}

			if (setPermissions)
				sql = sql.Replace("#USER#", _grantedUser).Replace("#DBNAME#", _databaseName);

			var sbDescription = new StringBuilder();
			var fileName = Path.GetFileNameWithoutExtension(file);
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

			using (var tran = _connection!.BeginTransaction())
			{
				try
				{
					var cmd = new SqlCommand(sql, _connection, tran);
					cmd.ExecuteNonQuery();

					cmd = new SqlCommand($"INSERT INTO {_dbPatchSchemaTable} ({COLUMN_PatchIdentifier}, {COLUMN_Version}, {COLUMN_Description}, {COLUMN_Created}, {COLUMN_FilePath}, {COLUMN_Script}, {COLUMN_Hash}) VALUES (@patchIdentifier, @ver, @description, @created, @filePath, @script, @hash)", _connection, tran);
					cmd.Parameters.AddWithValue("patchIdentifier", (object)patchDate ?? DBNull.Value);
					cmd.Parameters.AddWithValue("ver", version);
					cmd.Parameters.AddWithValue("description", (object?)description ?? DBNull.Value);
					cmd.Parameters.AddWithValue("created", GlobalContext.Instance.Now);
					cmd.Parameters.AddWithValue("filePath", file);
					cmd.Parameters.AddWithValue("script", (object)sql ?? DBNull.Value);
					cmd.Parameters.AddWithValue("hash", (object)hash ?? DBNull.Value);
					var result = (long)cmd.ExecuteNonQuery();

					if (result == 0)
						throw new InvalidOperationException($"No patch was inserted into {_dbPatchSchemaTable}");

					MoveFileToArchive(file);

					tran.Commit();
				}
				catch
				{
					tran.Rollback();
					throw;
				}
			}

			_logger.AppendLine($"Successfully patched: {file}");
		}
		catch (Exception ex)
		{
			_logger.AppendLine($"{nameof(SqlPatchManager)}.{nameof(Patch)}({nameof(file)} = {file})");
			_logger.AppendLine(ex.ToStringTrace());
			_succeeded = false;
			throw;
		}

		return version;
	}

	private void MoveFileToArchive(string filePath)
	{
		if (!_archive)
		{
			//_logger.LogInfo($"SKIP file archivation {filePath}");
			return;
		}

		try
		{
			var fileName = Path.GetFileName(filePath);
			var dir = Path.GetDirectoryName(filePath);
			dir = Path.Combine(dir!, "Archive");
			if (!Directory.Exists(dir))
				Directory.CreateDirectory(dir);

			var newFilePath = Path.Combine(dir, fileName);
			File.Move(filePath, newFilePath);

			_logger.AppendLine($"Archived file to {newFilePath}");
		}
		catch (Exception ex)
		{
			_logger.AppendLine($"{nameof(SqlPatchManager)}.{nameof(MoveFileToArchive)}");
			_logger.AppendLine(ex.ToStringTrace());
			_succeeded = false;
		}
	}
}
