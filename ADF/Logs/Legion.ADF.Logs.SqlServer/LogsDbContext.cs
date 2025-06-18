using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.Logs.SqlServer;

public partial class LogsDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.Logs.SqlServer.ILogsDbContext
{
	public static readonly System.Collections.Generic.IReadOnlyDictionary<string, string> PrimaryKeyFormatters;

	static LogsDbContext()
	{
		PrimaryKeyFormatters = new System.Collections.Generic.Dictionary<string, string>
		{
			{ nameof(Legion.ADF.Logs.Model.EnvironmentInfo), SqlServer.EnvironmentInfoConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Logs.Model.EventCounter), SqlServer.EventCounterConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Logs.Model.EventCounterCategory), SqlServer.EventCounterCategoryConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Logs.Model.EventCounterData), SqlServer.EventCounterDataConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Logs.Model.LocalRequest), SqlServer.LocalRequestConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Logs.Model.LocalRequestPayload), SqlServer.LocalRequestPayloadConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Logs.Model.LocalResponse), SqlServer.LocalResponseConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Logs.Model.LocalResponsePayload), SqlServer.LocalResponsePayloadConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Logs.Model.Log), SqlServer.LogConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Logs.Model.LogLevel), SqlServer.LogLevelConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Logs.Model.RemoteRequest), SqlServer.RemoteRequestConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Logs.Model.RemoteRequestPayload), SqlServer.RemoteRequestPayloadConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Logs.Model.RemoteResponse), SqlServer.RemoteResponseConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Logs.Model.RemoteResponsePayload), SqlServer.RemoteResponsePayloadConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Logs.Model.RemoteSystem), SqlServer.RemoteSystemConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Logs.Model.UnstructuredLog), SqlServer.UnstructuredLogConfiguration.PrimaryKeyFormatter },
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
			optionsBuilder.AddInterceptors(new Legion.EntityFrameworkCore.Interceptors.RowLockInterceptor_SqlServer());
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		RegisterUnaccentFunction(modelBuilder);

		SqlServer.EnvironmentInfoConfiguration.Build(modelBuilder);
		SqlServer.EventCounterConfiguration.Build(modelBuilder);
		SqlServer.EventCounterCategoryConfiguration.Build(modelBuilder);
		SqlServer.EventCounterDataConfiguration.Build(modelBuilder);
		SqlServer.LocalRequestConfiguration.Build(modelBuilder);
		SqlServer.LocalRequestPayloadConfiguration.Build(modelBuilder);
		SqlServer.LocalResponseConfiguration.Build(modelBuilder);
		SqlServer.LocalResponsePayloadConfiguration.Build(modelBuilder);
		SqlServer.LogConfiguration.Build(modelBuilder);
		SqlServer.LogLevelConfiguration.Build(modelBuilder);
		SqlServer.RemoteRequestConfiguration.Build(modelBuilder);
		SqlServer.RemoteRequestPayloadConfiguration.Build(modelBuilder);
		SqlServer.RemoteResponseConfiguration.Build(modelBuilder);
		SqlServer.RemoteResponsePayloadConfiguration.Build(modelBuilder);
		SqlServer.RemoteSystemConfiguration.Build(modelBuilder);
		SqlServer.UnstructuredLogConfiguration.Build(modelBuilder);
	}
}
