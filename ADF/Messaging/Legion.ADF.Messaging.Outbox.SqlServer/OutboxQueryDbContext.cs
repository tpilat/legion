using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.Messaging.Outbox.SqlServer;

public partial class OutboxQueryDbContext : Legion.EntityFrameworkCore.DbContextBase, Legion.ADF.Messaging.Outbox.SqlServer.IOutboxQueryDbContext
{
	public virtual DbSet<Legion.ADF.Messaging.Outbox.Model.VwBlockedOutboxMessageType> VwBlockedOutboxMessageType { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage> VwOutboxMessage { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive> VwOutboxMessageArchive { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent> VwOutboxMessageContent { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageProcessingLog> VwOutboxMessageProcessingLog { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue> VwOutboxQueue { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueueMessages> VwOutboxQueueMessages { get; set; }

	public OutboxQueryDbContext(DbContextOptions<OutboxQueryDbContext> options, Microsoft.Extensions.Logging.ILogger<OutboxQueryDbContext> logger)
		: base(options, logger)
	{
	}

	public OutboxQueryDbContext(Microsoft.Extensions.Logging.ILogger<OutboxQueryDbContext> logger)
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

		SqlServer.VwBlockedOutboxMessageTypeConfiguration.Build(modelBuilder);
		SqlServer.VwOutboxMessageConfiguration.Build(modelBuilder);
		SqlServer.VwOutboxMessageArchiveConfiguration.Build(modelBuilder);
		SqlServer.VwOutboxMessageContentConfiguration.Build(modelBuilder);
		SqlServer.VwOutboxMessageProcessingLogConfiguration.Build(modelBuilder);
		SqlServer.VwOutboxQueueConfiguration.Build(modelBuilder);
		SqlServer.VwOutboxQueueMessagesConfiguration.Build(modelBuilder);
	}
}
