using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.DomainEvents.SqlServer;

public class DomainEventProcessingStatusConfiguration : IEntityTypeConfiguration<DomainEvents.Model.DomainEventProcessingStatus>
{
	public const string PrimaryKeyFormatter = "{{\"IdDomainEventProcessingStatus\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<DomainEvents.Model.DomainEventProcessingStatus> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<DomainEvents.Model.DomainEventProcessingStatus> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdDomainEventProcessingStatus);

		entityBuilder.ToTable("DomainEventProcessingStatus", "devt");

		entityBuilder.Property(e => e.IdDomainEventProcessingStatus)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("nvarchar(63)")
			.HasMaxLength(63);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("nvarchar(127)")
			.HasMaxLength(127);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<DomainEvents.Model.DomainEventProcessingStatus>(ConfigureEntity);

		return modelBuilder;
	}
}
