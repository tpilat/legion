using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL;

public class VwMessageContentConfiguration : IEntityTypeConfiguration<MessageBox.Model.VwMessageContent>
{
	public void Configure(EntityTypeBuilder<MessageBox.Model.VwMessageContent> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.VwMessageContent> entityBuilder)
	{
		entityBuilder.ToView("VwMessageContent", "mbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdMessageContent).HasColumnType("uuid");

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
		=> modelBuilder.Entity<MessageBox.Model.VwMessageContent>(ConfigureEntity);
}
