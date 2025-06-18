using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Outbox.SqlServer;

public class VwBlockedOutboxMessageTypeConfiguration : IEntityTypeConfiguration<Outbox.Model.VwBlockedOutboxMessageType>
{
	public void Configure(EntityTypeBuilder<Outbox.Model.VwBlockedOutboxMessageType> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Outbox.Model.VwBlockedOutboxMessageType> entityBuilder)
	{
		entityBuilder.ToView("VwBlockedOutboxMessageType", "outbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdBlockedOutboxMessageType).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.Namespace)
			.IsRequired()
			.HasColumnType("nvarchar(1023)");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.IdOutboxInstance).HasColumnType("uniqueidentifier");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Outbox.Model.VwBlockedOutboxMessageType>(ConfigureEntity);
}
