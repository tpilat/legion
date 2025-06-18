using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Outbox.PostgreSQL;

public class OutboxMessageProcessingLogConfiguration : IEntityTypeConfiguration<Outbox.Model.OutboxMessageProcessingLog>
{
	public const string PrimaryKeyFormatter = "{{\"IdOutboxMessageProcessingLog\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Outbox.Model.OutboxMessageProcessingLog> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Outbox.Model.OutboxMessageProcessingLog> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdOutboxMessageProcessingLog);

		entityBuilder.ToTable("OutboxMessageProcessingLog", "outbox");

		entityBuilder.HasIndex(e => e.IdOutboxInstance, "IXFK_OutboxMessageProcessingLog_OutboxInstance");

		entityBuilder.HasIndex(e => e.IdOutboxMessage, "IX_OutboxMessageProcessingLog_IdOutboxMessage");

		entityBuilder.HasIndex(e => e.IdOutboxMessageStatus, "IXFK_OutboxMessageProcessingLog_OutboxMessageStatus");

		entityBuilder.HasIndex(e => e.IdOutboxQueue, "IXFK_OutboxMessageProcessingLog_OutboxQueue");

		entityBuilder.Property(e => e.IdOutboxMessageProcessingLog)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdOutboxMessage).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdOutboxQueue).HasColumnType("uuid");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.IdOutboxMessageStatus).HasColumnType("uuid");

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdLogMessage).HasColumnType("uuid");

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("varchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.IdOutboxInstance).HasColumnType("uuid");

		entityBuilder.HasOne(d => d.OutboxInstance)
			.WithMany(p => p.OutboxMessageProcessingLogs)
			.HasForeignKey(d => d.IdOutboxInstance)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OutboxMessageProcessingLog_IdOutboxInstance");

		entityBuilder.HasOne(d => d.OutboxMessageStatus)
			.WithMany(p => p.OutboxMessageProcessingLogs)
			.HasForeignKey(d => d.IdOutboxMessageStatus)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OutboxMessageProcessingLog_IdOutboxMessageStatus");

		entityBuilder.HasOne(d => d.OutboxQueue)
			.WithMany(p => p.OutboxMessageProcessingLogs)
			.HasForeignKey(d => d.IdOutboxQueue)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OutboxMessageProcessingLog_IdOutboxQueue");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Outbox.Model.OutboxMessageProcessingLog>(ConfigureEntity);

		return modelBuilder;
	}
}
