using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Inbox.PostgreSQL;

public class VwInboxMessageContentConfiguration : IEntityTypeConfiguration<Inbox.Model.VwInboxMessageContent>
{
	public void Configure(EntityTypeBuilder<Inbox.Model.VwInboxMessageContent> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Inbox.Model.VwInboxMessageContent> entityBuilder)
	{
		entityBuilder.ToView("VwInboxMessageContent", "inbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdInboxMessageContent).HasColumnType("uuid");

		entityBuilder.Property(e => e.MimeType)
			.IsRequired()
			.HasColumnType("varchar(1023)");

		entityBuilder.Property(e => e.ContentEncoding).HasColumnType("varchar(63)");

		entityBuilder.Property(e => e.ByteArrayContent).HasColumnType("bytea");

		entityBuilder.Property(e => e.JsonContent).HasColumnType("jsonb");

		entityBuilder.Property(e => e.Name).HasColumnType("varchar(511)");

		entityBuilder.Property(e => e.RelativePath).HasColumnType("varchar(1023)");

		entityBuilder.Property(e => e.Metadata).HasColumnType("jsonb");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Inbox.Model.VwInboxMessageContent>(ConfigureEntity);
}
