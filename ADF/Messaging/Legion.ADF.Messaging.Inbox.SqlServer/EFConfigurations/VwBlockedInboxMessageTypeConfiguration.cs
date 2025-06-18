using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Inbox.SqlServer;

public class VwBlockedInboxMessageTypeConfiguration : IEntityTypeConfiguration<Inbox.Model.VwBlockedInboxMessageType>
{
	public void Configure(EntityTypeBuilder<Inbox.Model.VwBlockedInboxMessageType> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Inbox.Model.VwBlockedInboxMessageType> entityBuilder)
	{
		entityBuilder.ToView("VwBlockedInboxMessageType", "inbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdBlockedInboxMessageType).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.Namespace)
			.IsRequired()
			.HasColumnType("nvarchar(1023)");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.IdInboxInstance).HasColumnType("uniqueidentifier");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Inbox.Model.VwBlockedInboxMessageType>(ConfigureEntity);
}
