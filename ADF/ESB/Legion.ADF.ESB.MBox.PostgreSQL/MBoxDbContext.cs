using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.ESB.MBox.PostgreSQL;

public partial class MBoxDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.ESB.MBox.PostgreSQL.IMBoxDbContext
{
	public static readonly System.Collections.Generic.IReadOnlyDictionary<string, string> PrimaryKeyFormatters;

	static MBoxDbContext()
	{
		PrimaryKeyFormatters = new System.Collections.Generic.Dictionary<string, string>
		{
			{ nameof(Legion.ADF.ESB.MBox.Model.Message), PostgreSQL.MessageConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ESB.MBox.Model.MessageContent), PostgreSQL.MessageContentConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ESB.MBox.Model.MessageProcessingLog), PostgreSQL.MessageProcessingLogConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ESB.MBox.Model.MessageProcessingStatus), PostgreSQL.MessageProcessingStatusConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ESB.MBox.Model.MessagePublishing), PostgreSQL.MessagePublishingConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ESB.MBox.Model.MessageStatus), PostgreSQL.MessageStatusConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ESB.MBox.Model.MessageType), PostgreSQL.MessageTypeConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ESB.MBox.Model.Queue), PostgreSQL.QueueConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ESB.MBox.Model.QueuedMessage), PostgreSQL.QueuedMessageConfiguration.PrimaryKeyFormatter },
		};
	}

	public virtual DbSet<Legion.ADF.ESB.MBox.Model.Message> Message { get; set; }
	public virtual DbSet<Legion.ADF.ESB.MBox.Model.MessageContent> MessageContent { get; set; }
	public virtual DbSet<Legion.ADF.ESB.MBox.Model.MessageProcessingLog> MessageProcessingLog { get; set; }
	public virtual DbSet<Legion.ADF.ESB.MBox.Model.MessageProcessingStatus> MessageProcessingStatus { get; set; }
	public virtual DbSet<Legion.ADF.ESB.MBox.Model.MessagePublishing> MessagePublishing { get; set; }
	public virtual DbSet<Legion.ADF.ESB.MBox.Model.MessageStatus> MessageStatus { get; set; }
	public virtual DbSet<Legion.ADF.ESB.MBox.Model.MessageType> MessageType { get; set; }
	public virtual DbSet<Legion.ADF.ESB.MBox.Model.Queue> Queue { get; set; }
	public virtual DbSet<Legion.ADF.ESB.MBox.Model.QueuedMessage> QueuedMessage { get; set; }

	public MBoxDbContext(DbContextOptions<MBoxDbContext> options, Microsoft.Extensions.Logging.ILogger<MBoxDbContext> logger)
		: base(options, logger)
	{
	}

	public MBoxDbContext(Microsoft.Extensions.Logging.ILogger<MBoxDbContext> logger)
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
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		RegisterUnaccentFunction(modelBuilder);

		PostgreSQL.MessageConfiguration.Build(modelBuilder);
		PostgreSQL.MessageContentConfiguration.Build(modelBuilder);
		PostgreSQL.MessageProcessingLogConfiguration.Build(modelBuilder);
		PostgreSQL.MessageProcessingStatusConfiguration.Build(modelBuilder);
		PostgreSQL.MessagePublishingConfiguration.Build(modelBuilder);
		PostgreSQL.MessageStatusConfiguration.Build(modelBuilder);
		PostgreSQL.MessageTypeConfiguration.Build(modelBuilder);
		PostgreSQL.QueueConfiguration.Build(modelBuilder);
		PostgreSQL.QueuedMessageConfiguration.Build(modelBuilder);
	}
}
