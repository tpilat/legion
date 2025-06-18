using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.DomainEvents.SqlServer;

public class VwDomainEventConfiguration : IEntityTypeConfiguration<DomainEvents.Model.VwDomainEvent>
{
	public void Configure(EntityTypeBuilder<DomainEvents.Model.VwDomainEvent> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<DomainEvents.Model.VwDomainEvent> entityBuilder)
	{
		entityBuilder.ToView("VwDomainEvent", "devt")
			.HasNoKey();

		entityBuilder.Property(e => e.IdDomainEvent).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdContent).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdDomainEventProcessingStatus).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.Namespace)
			.IsRequired()
			.HasColumnType("nvarchar(1023)");

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.Properties).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.Publisher).HasColumnType("nvarchar(511)");

		entityBuilder.Property(e => e.PublisherId).HasColumnType("nvarchar(511)");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.ProcessedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.SuspendedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.LastProcessingUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.LastProcessingTimeoutUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.NextProcessingUtc).HasColumnType("datetime2(7)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<DomainEvents.Model.VwDomainEvent>(ConfigureEntity);
}
