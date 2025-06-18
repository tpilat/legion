using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Inbox.PostgreSQL;

public class InboxMessageContentConfiguration : IEntityTypeConfiguration<Inbox.Model.InboxMessageContent>
{
	public const string PrimaryKeyFormatter = "{{\"IdInboxMessageContent\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Inbox.Model.InboxMessageContent> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Inbox.Model.InboxMessageContent> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdInboxMessageContent);

		entityBuilder.ToTable("InboxMessageContent", "inbox");

		entityBuilder.Property(e => e.IdInboxMessageContent)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.MimeType)
			.IsRequired()
			.HasColumnType("varchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.ContentEncoding)
			.HasColumnType("varchar(63)")
			.HasMaxLength(63);

		entityBuilder.Property(e => e.ByteArrayContent).HasColumnType("bytea");

		entityBuilder.Property(e => e.JsonContent).HasColumnType("jsonb");

		entityBuilder.Property(e => e.Name)
			.HasColumnType("varchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.RelativePath)
			.HasColumnType("varchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.Metadata).HasColumnType("jsonb");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Inbox.Model.InboxMessageContent>(ConfigureEntity);

		return modelBuilder;
	}
}
