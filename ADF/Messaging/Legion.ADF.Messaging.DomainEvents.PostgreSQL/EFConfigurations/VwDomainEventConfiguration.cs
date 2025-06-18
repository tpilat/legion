using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.DomainEvents.PostgreSQL;

public class VwDomainEventConfiguration : IEntityTypeConfiguration<DomainEvents.Model.VwDomainEvent>
{
	public void Configure(EntityTypeBuilder<DomainEvents.Model.VwDomainEvent> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<DomainEvents.Model.VwDomainEvent> entityBuilder)
	{
		entityBuilder.ToView("VwDomainEvent", "devt")
			.HasNoKey();

		entityBuilder.Property(e => e.IdDomainEvent).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdContent).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdDomainEventProcessingStatus).HasColumnType("uuid");

		entityBuilder.Property(e => e.Namespace).HasColumnType("varchar(1023)");

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uuid");

		entityBuilder.Property(e => e.Properties).HasColumnType("jsonb");

		entityBuilder.Property(e => e.Publisher).HasColumnType("varchar(511)");

		entityBuilder.Property(e => e.PublisherId).HasColumnType("varchar(511)");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.ProcessedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.SuspendedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.LastProcessingUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.LastProcessingTimeoutUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.NextProcessingUtc).HasColumnType("timestamptz");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<DomainEvents.Model.VwDomainEvent>(ConfigureEntity);
}
