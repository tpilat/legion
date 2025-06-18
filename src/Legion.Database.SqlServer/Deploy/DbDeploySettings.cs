using Legion.Extensions;
using Legion.IOUtils;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Legion.Validation.Results;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace Legion.Database.SqlServer.Deploy;

public class DbDeploySettings
{
	private const string ERROR_FILE_NAME = "error.txt";

	public string? _workingDirectory;
	public string WorkingDirectory
	{
		get
		{
			if (string.IsNullOrWhiteSpace(_workingDirectory))
				_workingDirectory = AppDomain.CurrentDomain.BaseDirectory;

			return _workingDirectory!;
		}
		set
		{
			_workingDirectory = value;
		}
	}

	public string? _errorFilePath;
	public string ErrorFilePath
	{
		get
		{
			if (string.IsNullOrWhiteSpace(_errorFilePath))
			{
				_errorFilePath = Path.Combine(WorkingDirectory.TrimPostfix(DirectoryHelper.DirectorySeparatorCharAsString), ERROR_FILE_NAME);
			}
			else if (!_errorFilePath!.StartsWith(WorkingDirectory, StringComparison.InvariantCultureIgnoreCase))
			{
				_errorFilePath = DirectoryHelper.CombinePaths(WorkingDirectory, _errorFilePath, true);
			}

			return _errorFilePath!;
		}
		set
		{
			_errorFilePath = value;
		}
	}

	public string ConncetionString { get; set; }
	public string AdminUser { get; private set; }
	public string TargetDatabase { get; private set; }
	public string TargetDbUsername { get; set; }
	public string TargetDbPassword { get; set; }
	public bool UseGlobalTransaction { get; set; }
	public bool MergeAllSqlFiles { get; set; }
	public List<SqlFileSettings> SqlFiles { get; set; }
	public DbPatchSettings? DbPatch { get; set; }

	private bool _initialized;
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

			if (string.IsNullOrWhiteSpace(ConncetionString))
				return;

			var builder = new SqlConnectionStringBuilder(ConncetionString);
			AdminUser = builder.UserID!;
			TargetDatabase = builder.InitialCatalog!;

			if (0 < SqlFiles?.Count)
			{
				foreach (var sqlFile in SqlFiles)
				{
					if (string.IsNullOrWhiteSpace(sqlFile.CurrentDatabase))
						sqlFile.CurrentDatabase = TargetDatabase;
				}
			}

			if (0 < DbPatch?.SqlFiles?.Count)
			{
				foreach (var sqlFile in DbPatch.SqlFiles)
				{
					if (string.IsNullOrWhiteSpace(sqlFile.CurrentDatabase))
						sqlFile.CurrentDatabase = TargetDatabase;
				}
			}
		}
	}

	public class Validator : ValidatorBase<DbDeploySettings>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<DbDeploySettings> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<DbDeploySettings> builder)
		{
			builder?
				.ForProperty(x => x.ConncetionString, v => v.NotDefaultOrWhiteSpace())
				.ForProperty(x => x.AdminUser, v => v.NotDefaultOrWhiteSpace())
				.ForProperty(x => x.TargetDatabase, v => v.NotDefaultOrWhiteSpace())
				.ForProperty(x => x.TargetDbUsername, v => v.NotDefaultOrWhiteSpace())
				.ForProperty(x => x.TargetDbPassword, v => v.NotDefaultOrWhiteSpace())
				.ForProperty(x => x.SqlFiles, v => v.NotDefaultOrEmpty())
				.ForEach(x => x.SqlFiles, SqlFileSettings.Validator.RulesBuilder)
				.ForNavigation(x => x.DbPatch, DbPatchSettings.Validator.RulesBuilder!)
				.WithPropertyError(x => x.ConncetionString, (obj, parent) =>
				{
					if (obj != null)
					{
						try
						{
							var connStringBuilder = new SqlConnectionStringBuilder(obj.ConncetionString);
						}
						catch (Exception ex)
						{
							return ValidationResultFactory.Failure(
								obj,
								x => x.ConncetionString,
								objectPathIndexes: null,
								Exceptions.Internal.ErrorCodes.ConncetionStringException.Default,
								nameof(ConncetionString),
								ex.ToStringTrace());
						}
					}

					return ValidationResultFactory.Success();
				})
			;
		}

		public override IValidationResult Validate(DbDeploySettings? obj, Dictionary<int, int>? indexes = null, ValidationOptions? options = null)
		{
			obj?.Initialize();
			return base.Validate(obj, indexes, options);
		}

		public override IValidationResult Validate(DbDeploySettings? obj, int? index, ValidationOptions? options = null)
		{
			obj?.Initialize();
			return base.Validate(obj, index, options);
		}
	}

	public class DbPatchSettings
	{
		public string PatchDatabaseName { get; set; }
		public string PatchSchemaName { get; set; }
		public string PatchTableName { get; set; }
		public string PreviousVersion { get; set; }
		public DateTime? FromDateTime { get; set; }
		public bool Archive { get; set; }
		public List<string> AllowedConditions { get; set; }
		public bool MergeAllSqlFiles { get; set; }
		public List<SqlFileSettings> SqlFiles { get; set; }

		public class Validator : ValidatorBase<DbPatchSettings>
		{
			public Validator() { }
			public Validator(IObjectPath objectPath) : base(objectPath) { }

			public override void SetDefaultRuels(ValidatorBuilder<DbPatchSettings> builder)
				=> RulesBuilder(builder);

			public static void RulesBuilder(ValidatorBuilder<DbPatchSettings> builder)
			{
				builder?
					.ForProperty(x => x.PatchDatabaseName, v => v.NotDefaultOrWhiteSpace())
					.ForProperty(x => x.PatchSchemaName, v => v.NotDefaultOrWhiteSpace())
					.ForProperty(x => x.PatchTableName, v => v.NotDefaultOrWhiteSpace())
					.ForProperty(x => x.SqlFiles, v => v.NotDefaultOrEmpty())
					.ForEach(x => x.SqlFiles, SqlFileSettings.Validator.RulesBuilder);
			}
		}
	}

	public enum TransactionMode
	{
		None = 0,
		Global = 1,
		Local = 2
	}

	public class SqlFileSettings
	{
		public Encoding? _encoding;
		public Encoding Encoding
		{
			get
			{
				_encoding ??= GlobalCache.UTF8NoBOM;
				return _encoding!;
			}
			set
			{
				_encoding = value;
			}
		}

		public string FilePath { get; set; }
		public TransactionMode? Transaction { get; set; }
		public string CurrentDatabase { get; set; }
		public int? CommandTimeout { get; set; }
		public string? FilledScriptPostfix { get; set; }

		public class Validator : ValidatorBase<SqlFileSettings>
		{
			public Validator() { }
			public Validator(IObjectPath objectPath) : base(objectPath) { }

			public override void SetDefaultRuels(ValidatorBuilder<SqlFileSettings> builder)
				=> RulesBuilder(builder);

			public static void RulesBuilder(ValidatorBuilder<SqlFileSettings> builder)
			{
				builder?
					.ForProperty(x => x.FilePath, v => v.NotDefaultOrWhiteSpace())
					.ForProperty(x => x.CurrentDatabase, v => v.NotDefaultOrWhiteSpace())
					.ForProperty(x => x.CommandTimeout, v => v.GreaterThan(0))
					.ForProperty(x => x.Transaction, v => v.NotEqualsTo(TransactionMode.Global), (obj, parent) =>
					{
						if (parent?.Instance is DbDeploySettings dbDeploySettings)
							return dbDeploySettings.UseGlobalTransaction == true;

						if (parent?.Parent?.Instance is DbDeploySettings dbDeploySettings2)
							return dbDeploySettings2.UseGlobalTransaction == true;

						return false;
					});
			}
		}
	}
}

public static class DbDeploySettingsExtensions
{
	public static IServiceCollection AddSqlServerDbDeploySettings(
		this IServiceCollection services,
		Action<Microsoft.Extensions.Options.OptionsBuilder<DbDeploySettings>> bindConfiguration,
		string validatorBasePath)
	{
		Throw.IfArgumentNull(bindConfiguration);
		Throw.IfArgumentNullOrWhiteSpace(validatorBasePath);

		var optionsBuilder = services.AddOptions<DbDeploySettings>();

		//optionsBuilder.BindConfiguration(string configSectionPath)
		bindConfiguration.Invoke(optionsBuilder);

		optionsBuilder
			.AddOptionsValidator(validatorBasePath)
			.ValidateOnStart();

		return services;
	}
}
