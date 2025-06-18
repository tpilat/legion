using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Inbox.SqlServer;

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
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.MimeType)
			.IsRequired()
			.HasColumnType("nvarchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.ContentEncoding)
			.HasColumnType("nvarchar(63)")
			.HasMaxLength(63);

		entityBuilder.Property(e => e.ByteArrayContent).HasColumnType("varbinary(max)");

		entityBuilder.Property(e => e.JsonContent).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.StringContent).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.Name)
			.HasColumnType("nvarchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.RelativePath)
			.HasColumnType("nvarchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.Metadata).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.IsCompressed).HasColumnType("bit");

		entityBuilder.Property(e => e.EncryptionKey).HasColumnType("nvarchar(max)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Inbox.Model.InboxMessageContent>(ConfigureEntity);

		return modelBuilder;
	}
}
