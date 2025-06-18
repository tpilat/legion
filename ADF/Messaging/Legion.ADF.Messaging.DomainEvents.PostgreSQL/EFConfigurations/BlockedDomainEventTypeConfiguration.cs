using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.DomainEvents.PostgreSQL;

public class BlockedDomainEventTypeConfiguration : IEntityTypeConfiguration<DomainEvents.Model.BlockedDomainEventType>
{
	public const string PrimaryKeyFormatter = "{{\"IdBlockedDomainEventType\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<DomainEvents.Model.BlockedDomainEventType> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<DomainEvents.Model.BlockedDomainEventType> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdBlockedDomainEventType);

		entityBuilder.ToTable("BlockedDomainEventType", "devt");

		entityBuilder.HasIndex(e => e.Namespace, "UQ_BlockedDomainEventType_Namespace")
				.IsUnique();

		entityBuilder.Property(e => e.IdBlockedDomainEventType)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Namespace)
			.IsRequired()
			.HasColumnType("varchar(1023)")
			.HasMaxLength(1023);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<DomainEvents.Model.BlockedDomainEventType>(ConfigureEntity);

		return modelBuilder;
	}
}
