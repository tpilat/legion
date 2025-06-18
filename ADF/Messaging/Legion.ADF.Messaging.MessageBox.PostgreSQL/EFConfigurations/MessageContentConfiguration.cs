using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL;

public class MessageContentConfiguration : IEntityTypeConfiguration<MessageBox.Model.MessageContent>
{
	public const string PrimaryKeyFormatter = "{{\"IdMessageContent\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<MessageBox.Model.MessageContent> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.MessageContent> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdMessageContent);

		entityBuilder.ToTable("MessageContent", "mbox");

		entityBuilder.Property(e => e.IdMessageContent)
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
		modelBuilder.Entity<MessageBox.Model.MessageContent>(ConfigureEntity);

		return modelBuilder;
	}
}
