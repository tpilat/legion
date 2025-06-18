using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Inbox.SqlServer;

public class VwInboxMessageContentConfiguration : IEntityTypeConfiguration<Inbox.Model.VwInboxMessageContent>
{
	public void Configure(EntityTypeBuilder<Inbox.Model.VwInboxMessageContent> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Inbox.Model.VwInboxMessageContent> entityBuilder)
	{
		entityBuilder.ToView("VwInboxMessageContent", "inbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdInboxMessageContent).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.MimeType)
			.IsRequired()
			.HasColumnType("nvarchar(1023)");

		entityBuilder.Property(e => e.ContentEncoding).HasColumnType("nvarchar(63)");

		entityBuilder.Property(e => e.ByteArrayContent).HasColumnType("varbinary(max)");

		entityBuilder.Property(e => e.JsonContent).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.StringContent).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.Name).HasColumnType("nvarchar(511)");

		entityBuilder.Property(e => e.RelativePath).HasColumnType("nvarchar(1023)");

		entityBuilder.Property(e => e.Metadata).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.IsCompressed).HasColumnType("bit");

		entityBuilder.Property(e => e.EncryptionKey).HasColumnType("nvarchar(max)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Inbox.Model.VwInboxMessageContent>(ConfigureEntity);
}
