using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Inbox.PostgreSQL;

public class VwBlockedInboxMessageTypeConfiguration : IEntityTypeConfiguration<Inbox.Model.VwBlockedInboxMessageType>
{
	public void Configure(EntityTypeBuilder<Inbox.Model.VwBlockedInboxMessageType> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Inbox.Model.VwBlockedInboxMessageType> entityBuilder)
	{
		entityBuilder.ToView("VwBlockedInboxMessageType", "inbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdBlockedInboxMessageType).HasColumnType("uuid");

		entityBuilder.Property(e => e.Namespace)
			.IsRequired()
			.HasColumnType("varchar(1023)");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.IdInboxInstance).HasColumnType("uuid");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Inbox.Model.VwBlockedInboxMessageType>(ConfigureEntity);
}
