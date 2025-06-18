using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.DomainEvents.SqlServer;

public class DomainEventConfiguration : IEntityTypeConfiguration<DomainEvents.Model.DomainEvent>
{
	public const string PrimaryKeyFormatter = "{{\"IdDomainEvent\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<DomainEvents.Model.DomainEvent> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<DomainEvents.Model.DomainEvent> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdDomainEvent);

		entityBuilder.ToTable("DomainEvent", "devt");

		entityBuilder.HasIndex(e => e.IdContent, "IXFK_DomainEvent_DomainEventContent");

		entityBuilder.HasIndex(e => e.IdContent, "UQ_DomainEvent_IdContent")
				.IsUnique();

		entityBuilder.HasIndex(e => e.IdDomainEventProcessingStatus, "IXFK_DomainEvent_DomainEventProcessingStatus");

		entityBuilder.Property(e => e.IdDomainEvent)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdContent).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdDomainEventProcessingStatus).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.Namespace)
			.IsRequired()
			.HasColumnType("nvarchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.Properties).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.Publisher)
			.HasColumnType("nvarchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.PublisherId)
			.HasColumnType("nvarchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.ProcessedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.SuspendedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.LastProcessingUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.LastProcessingTimeoutUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.NextProcessingUtc).HasColumnType("datetime2(7)");

		entityBuilder.HasOne(d => d.Content)
			.WithOne(p => p.DomainEvent)
			.HasForeignKey<DomainEvents.Model.DomainEvent>(d => d.IdContent)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_DomainEvent_IdDomainEventContent");

		entityBuilder.HasOne(d => d.DomainEventProcessingStatus)
			.WithMany(p => p.DomainEvents)
			.HasForeignKey(d => d.IdDomainEventProcessingStatus)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_DomainEvent_IdDomainEventProcessingStatus");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<DomainEvents.Model.DomainEvent>(ConfigureEntity);

		return modelBuilder;
	}
}
