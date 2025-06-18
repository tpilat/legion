using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Outbox.PostgreSQL;

public class VwBlockedOutboxMessageTypeConfiguration : IEntityTypeConfiguration<Outbox.Model.VwBlockedOutboxMessageType>
{
	public void Configure(EntityTypeBuilder<Outbox.Model.VwBlockedOutboxMessageType> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Outbox.Model.VwBlockedOutboxMessageType> entityBuilder)
	{
		entityBuilder.ToView("VwBlockedOutboxMessageType", "outbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdBlockedOutboxMessageType).HasColumnType("uuid");

		entityBuilder.Property(e => e.Namespace)
			.IsRequired()
			.HasColumnType("varchar(1023)");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.IdOutboxInstance).HasColumnType("uuid");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Outbox.Model.VwBlockedOutboxMessageType>(ConfigureEntity);
}
