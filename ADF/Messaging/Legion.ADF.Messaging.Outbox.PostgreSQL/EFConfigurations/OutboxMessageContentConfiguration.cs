using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Outbox.PostgreSQL;

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
		modelBuilder.Entity<Outbox.Model.OutboxMessageContent>(ConfigureEntity);

		return modelBuilder;
	}
}
