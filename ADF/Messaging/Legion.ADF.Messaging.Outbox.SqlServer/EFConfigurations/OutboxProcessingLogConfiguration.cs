using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Outbox.SqlServer;

public class OutboxProcessingLogConfiguration : IEntityTypeConfiguration<Outbox.Model.OutboxProcessingLog>
{
	public const string PrimaryKeyFormatter = "{{\"IdOutboxProcessingLog\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Outbox.Model.OutboxProcessingLog> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Outbox.Model.OutboxProcessingLog> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdOutboxProcessingLog);

		entityBuilder.ToTable("OutboxProcessingLog", "outbox");

		entityBuilder.HasIndex(e => e.IdOutboxInstance, "IXFK_OutboxProcessingLog_OutboxInstance");

		entityBuilder.HasIndex(e => e.IdOutboxQueue, "IXFK_OutboxProcessingLog_OutboxQueue");

		entityBuilder.Property(e => e.IdOutboxProcessingLog)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdOutboxInstance).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdOutboxQueue).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdLogMessage).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("nvarchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.Detail).HasColumnType("nvarchar(max)");

		entityBuilder.HasOne(d => d.OutboxInstance)
			.WithMany(p => p.OutboxProcessingLogs)
			.HasForeignKey(d => d.IdOutboxInstance)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OutboxProcessingLog_IdOutboxInstance");

		entityBuilder.HasOne(d => d.OutboxQueue)
			.WithMany(p => p.OutboxProcessingLogs)
			.HasForeignKey(d => d.IdOutboxQueue)
			.HasConstraintName("FK_OutboxProcessingLog_OutboxQueue");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Outbox.Model.OutboxProcessingLog>(ConfigureEntity);

		return modelBuilder;
	}
}
