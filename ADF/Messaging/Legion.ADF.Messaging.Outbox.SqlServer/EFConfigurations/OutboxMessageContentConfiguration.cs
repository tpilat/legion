using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Outbox.SqlServer;

public class OutboxMessageContentConfiguration : IEntityTypeConfiguration<Outbox.Model.OutboxMessageContent>
{
	public const string PrimaryKeyFormatter = "{{\"IdOutboxMessageContent\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Outbox.Model.OutboxMessageContent> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Outbox.Model.OutboxMessageContent> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdOutboxMessageContent);

		entityBuilder.ToTable("OutboxMessageContent", "outbox");

		entityBuilder.Property(e => e.IdOutboxMessageContent)
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
		modelBuilder.Entity<Outbox.Model.OutboxMessageContent>(ConfigureEntity);

		return modelBuilder;
	}
}
