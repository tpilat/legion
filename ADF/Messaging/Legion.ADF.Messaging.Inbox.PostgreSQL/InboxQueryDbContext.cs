using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.Messaging.Inbox.PostgreSQL;

public partial class InboxQueryDbContext : Legion.EntityFrameworkCore.DbContextBase, Legion.ADF.Messaging.Inbox.PostgreSQL.IInboxQueryDbContext
{
	public virtual DbSet<Legion.ADF.Messaging.Inbox.Model.VwBlockedInboxMessageType> VwBlockedInboxMessageType { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Inbox.Model.VwInboxMessage> VwInboxMessage { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive> VwInboxMessageArchive { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent> VwInboxMessageContent { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog> VwInboxMessageProcessingLog { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Inbox.Model.VwInboxQueue> VwInboxQueue { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages> VwInboxQueueMessages { get; set; }

	public InboxQueryDbContext(DbContextOptions<InboxQueryDbContext> options, Microsoft.Extensions.Logging.ILogger<InboxQueryDbContext> logger)
		: base(options, logger)
	{
	}

	public InboxQueryDbContext(Microsoft.Extensions.Logging.ILogger<InboxQueryDbContext> logger)
		: base(logger)
	{
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
		
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

		PostgreSQL.VwBlockedInboxMessageTypeConfiguration.Build(modelBuilder);
		PostgreSQL.VwInboxMessageConfiguration.Build(modelBuilder);
		PostgreSQL.VwInboxMessageArchiveConfiguration.Build(modelBuilder);
		PostgreSQL.VwInboxMessageContentConfiguration.Build(modelBuilder);
		PostgreSQL.VwInboxMessageProcessingLogConfiguration.Build(modelBuilder);
		PostgreSQL.VwInboxQueueConfiguration.Build(modelBuilder);
		PostgreSQL.VwInboxQueueMessagesConfiguration.Build(modelBuilder);
	}
}
