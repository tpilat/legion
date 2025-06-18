using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.DomainEvents.SqlServer;

public class DomainEventProcessingLogConfiguration : IEntityTypeConfiguration<DomainEvents.Model.DomainEventProcessingLog>
{
	public const string PrimaryKeyFormatter = "{{\"IdDomainEventProcessingLog\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<DomainEvents.Model.DomainEventProcessingLog> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<DomainEvents.Model.DomainEventProcessingLog> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdDomainEventProcessingLog);

		entityBuilder.ToTable("DomainEventProcessingLog", "devt");

		entityBuilder.HasIndex(e => e.IdDomainEvent, "IXFK_DomainEventProcessingLog_DomainEvent");

		entityBuilder.HasIndex(e => e.IdDomainEventProcessingStatus, "IXFK_DomainEventProcessingLog_DomainEventProcessingStatus");

		entityBuilder.Property(e => e.IdDomainEventProcessingLog)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdDomainEvent).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.IdDomainEventProcessingStatus).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdLogMessage).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("nvarchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.Detail).HasColumnType("nvarchar(max)");

		entityBuilder.HasOne(d => d.DomainEvent)
			.WithMany(p => p.DomainEventProcessingLogs)
			.HasForeignKey(d => d.IdDomainEvent)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_DomainEventProcessingLog_IdDomainEvent");

		entityBuilder.HasOne(d => d.DomainEventProcessingStatus)
			.WithMany(p => p.DomainEventProcessingLogs)
			.HasForeignKey(d => d.IdDomainEventProcessingStatus)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_DomainEventProcessingLog_IdDomainEventProcessingStatus");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<DomainEvents.Model.DomainEventProcessingLog>(ConfigureEntity);

		return modelBuilder;
	}
}
