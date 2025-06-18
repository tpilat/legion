using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Inbox.PostgreSQL;

public class InboxMessageProcessingLogConfiguration : IEntityTypeConfiguration<Inbox.Model.InboxMessageProcessingLog>
{
	public const string PrimaryKeyFormatter = "{{\"IdInboxMessageProcessingLog\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Inbox.Model.InboxMessageProcessingLog> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Inbox.Model.InboxMessageProcessingLog> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdInboxMessageProcessingLog);

		entityBuilder.ToTable("InboxMessageProcessingLog", "inbox");

		entityBuilder.HasIndex(e => e.IdInboxInstance, "IXFK_InboxMessageProcessingLog_InboxInstance");

		entityBuilder.HasIndex(e => e.IdInboxMessage, "IX_InboxMessageProcessingLog_IdInboxMessage");

		entityBuilder.HasIndex(e => e.IdInboxMessageStatus, "IXFK_InboxMessageProcessingLog_InboxMessageStatus");

		entityBuilder.HasIndex(e => e.IdInboxQueue, "IXFK_InboxMessageProcessingLog_InboxQueue");

		entityBuilder.Property(e => e.IdInboxMessageProcessingLog)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdInboxMessage).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdInboxQueue).HasColumnType("uuid");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.IdInboxMessageStatus).HasColumnType("uuid");

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdLogMessage).HasColumnType("uuid");

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("varchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.IdInboxInstance).HasColumnType("uuid");

		entityBuilder.HasOne(d => d.InboxInstance)
			.WithMany(p => p.InboxMessageProcessingLogs)
			.HasForeignKey(d => d.IdInboxInstance)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_InboxMessageProcessingLog_IdInboxInstance");

		entityBuilder.HasOne(d => d.InboxMessageStatus)
			.WithMany(p => p.InboxMessageProcessingLogs)
			.HasForeignKey(d => d.IdInboxMessageStatus)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_InboxMessageProcessingLog_IdInboxMessageStatus");

		entityBuilder.HasOne(d => d.InboxQueue)
			.WithMany(p => p.InboxMessageProcessingLogs)
			.HasForeignKey(d => d.IdInboxQueue)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_InboxMessageProcessingLog_IdInboxQueue");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Inbox.Model.InboxMessageProcessingLog>(ConfigureEntity);

		return modelBuilder;
	}
}
