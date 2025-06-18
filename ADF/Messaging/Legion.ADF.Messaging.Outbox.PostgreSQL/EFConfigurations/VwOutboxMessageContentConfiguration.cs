using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Outbox.PostgreSQL;

public class VwOutboxMessageContentConfiguration : IEntityTypeConfiguration<Outbox.Model.VwOutboxMessageContent>
{
	public void Configure(EntityTypeBuilder<Outbox.Model.VwOutboxMessageContent> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Outbox.Model.VwOutboxMessageContent> entityBuilder)
	{
		entityBuilder.ToView("VwOutboxMessageContent", "outbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdOutboxMessageContent).HasColumnType("uuid");

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
		=> modelBuilder.Entity<Outbox.Model.VwOutboxMessageContent>(ConfigureEntity);
}
