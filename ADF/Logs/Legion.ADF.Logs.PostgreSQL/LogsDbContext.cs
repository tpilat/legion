using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.Logs.PostgreSQL;

public partial class LogsDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.Logs.PostgreSQL.ILogsDbContext
{
	public static readonly System.Collections.Generic.IReadOnlyDictionary<string, string> PrimaryKeyFormatters;

	static LogsDbContext()
	{
		PrimaryKeyFormatters = new System.Collections.Generic.Dictionary<string, string>
		{
			{ nameof(Legion.ADF.Logs.Model.EnvironmentInfo), PostgreSQL.EnvironmentInfoConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Logs.Model.EventCounter), PostgreSQL.EventCounterConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Logs.Model.EventCounterCategory), PostgreSQL.EventCounterCategoryConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Logs.Model.EventCounterData), PostgreSQL.EventCounterDataConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Logs.Model.LocalRequest), PostgreSQL.LocalRequestConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Logs.Model.LocalRequestPayload), PostgreSQL.LocalRequestPayloadConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Logs.Model.LocalResponse), PostgreSQL.LocalResponseConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Logs.Model.LocalResponsePayload), PostgreSQL.LocalResponsePayloadConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Logs.Model.Log), PostgreSQL.LogConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Logs.Model.LogLevel), PostgreSQL.LogLevelConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Logs.Model.RemoteRequest), PostgreSQL.RemoteRequestConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Logs.Model.RemoteRequestPayload), PostgreSQL.RemoteRequestPayloadConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Logs.Model.RemoteResponse), PostgreSQL.RemoteResponseConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Logs.Model.RemoteResponsePayload), PostgreSQL.RemoteResponsePayloadConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Logs.Model.RemoteSystem), PostgreSQL.RemoteSystemConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Logs.Model.UnstructuredLog), PostgreSQL.UnstructuredLogConfiguration.PrimaryKeyFormatter },
		};
	}

	public virtual DbSet<Legion.ADF.Logs.Model.EnvironmentInfo> EnvironmentInfo { get; set; }
	public virtual DbSet<Legion.ADF.Logs.Model.EventCounter> EventCounter { get; set; }
	public virtual DbSet<Legion.ADF.Logs.Model.EventCounterCategory> EventCounterCategory { get; set; }
	public virtual DbSet<Legion.ADF.Logs.Model.EventCounterData> EventCounterData { get; set; }
	public virtual DbSet<Legion.ADF.Logs.Model.LocalRequest> LocalRequest { get; set; }
	public virtual DbSet<Legion.ADF.Logs.Model.LocalRequestPayload> LocalRequestPayload { get; set; }
	public virtual DbSet<Legion.ADF.Logs.Model.LocalResponse> LocalResponse { get; set; }
	public virtual DbSet<Legion.ADF.Logs.Model.LocalResponsePayload> LocalResponsePayload { get; set; }
	public virtual DbSet<Legion.ADF.Logs.Model.Log> Log { get; set; }
	public virtual DbSet<Legion.ADF.Logs.Model.LogLevel> LogLevel { get; set; }
	public virtual DbSet<Legion.ADF.Logs.Model.RemoteRequest> RemoteRequest { get; set; }
	public virtual DbSet<Legion.ADF.Logs.Model.RemoteRequestPayload> RemoteRequestPayload { get; set; }
	public virtual DbSet<Legion.ADF.Logs.Model.RemoteResponse> RemoteResponse { get; set; }
	public virtual DbSet<Legion.ADF.Logs.Model.RemoteResponsePayload> RemoteResponsePayload { get; set; }
	public virtual DbSet<Legion.ADF.Logs.Model.RemoteSystem> RemoteSystem { get; set; }
	public virtual DbSet<Legion.ADF.Logs.Model.UnstructuredLog> UnstructuredLog { get; set; }

	public LogsDbContext(DbContextOptions<LogsDbContext> options, Microsoft.Extensions.Logging.ILogger<LogsDbContext> logger)
		: base(options, logger)
	{
	}

	public LogsDbContext(Microsoft.Extensions.Logging.ILogger<LogsDbContext> logger)
		: base(logger)
	{
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
		{
			if (ConnectionProvider == null)
				Legion.Throw.InitializationException(ConnectionProvider);

			ConnectionProvider.OnConfiguring(optionsBuilder);
		}
		else
		{
			SetIsDbContextOptionsBuilderPreconfigured();
		}

		if (DbContextSettintgs.AllowLocking == true)
			optionsBuilder.AddInterceptors(new Legion.EntityFrameworkCore.Interceptors.RowLockInterceptor_PostgreSql());
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		RegisterUnaccentFunction(modelBuilder);

		PostgreSQL.EnvironmentInfoConfiguration.Build(modelBuilder);
		PostgreSQL.EventCounterConfiguration.Build(modelBuilder);
		PostgreSQL.EventCounterCategoryConfiguration.Build(modelBuilder);
		PostgreSQL.EventCounterDataConfiguration.Build(modelBuilder);
		PostgreSQL.LocalRequestConfiguration.Build(modelBuilder);
		PostgreSQL.LocalRequestPayloadConfiguration.Build(modelBuilder);
		PostgreSQL.LocalResponseConfiguration.Build(modelBuilder);
		PostgreSQL.LocalResponsePayloadConfiguration.Build(modelBuilder);
		PostgreSQL.LogConfiguration.Build(modelBuilder);
		PostgreSQL.LogLevelConfiguration.Build(modelBuilder);
		PostgreSQL.RemoteRequestConfiguration.Build(modelBuilder);
		PostgreSQL.RemoteRequestPayloadConfiguration.Build(modelBuilder);
		PostgreSQL.RemoteResponseConfiguration.Build(modelBuilder);
		PostgreSQL.RemoteResponsePayloadConfiguration.Build(modelBuilder);
		PostgreSQL.RemoteSystemConfiguration.Build(modelBuilder);
		PostgreSQL.UnstructuredLogConfiguration.Build(modelBuilder);
	}
}
