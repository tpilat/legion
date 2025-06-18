using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.SqlServer;

public class VwBlockedMessageTypeConfiguration : IEntityTypeConfiguration<MessageBox.Model.VwBlockedMessageType>
{
	public void Configure(EntityTypeBuilder<MessageBox.Model.VwBlockedMessageType> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.VwBlockedMessageType> entityBuilder)
	{
		entityBuilder.ToView("VwBlockedMessageType", "mbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdBlockedMessageType).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.Namespace)
			.IsRequired()
			.HasColumnType("nvarchar(1023)");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.IdMessageBoxInstance).HasColumnType("uniqueidentifier");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<MessageBox.Model.VwBlockedMessageType>(ConfigureEntity);
}
