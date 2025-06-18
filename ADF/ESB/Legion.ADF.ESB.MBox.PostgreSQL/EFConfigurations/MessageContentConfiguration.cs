using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.MBox.PostgreSQL;

public class MessageContentConfiguration : IEntityTypeConfiguration<MBox.Model.MessageContent>
{
	public const string PrimaryKeyFormatter = "{{\"IdMessageContent\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<MBox.Model.MessageContent> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MBox.Model.MessageContent> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdMessageContent);

		entityBuilder.ToTable("MessageContent", "mbox");

		entityBuilder.Property(e => e.IdMessageContent).ValueGeneratedNever();

		entityBuilder.Property(e => e.ContentType)
			.IsRequired()
			.HasMaxLength(255);

		entityBuilder.Property(e => e.ContentEncoding).HasMaxLength(63);

		entityBuilder.Property(e => e.JsonContent).HasColumnType("jsonb");

		entityBuilder.Property(e => e.Name).HasMaxLength(511);

		entityBuilder.Property(e => e.RelativePath).HasMaxLength(1023);

		entityBuilder.Property(e => e.Metadata).HasColumnType("jsonb");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<MBox.Model.MessageContent>(ConfigureEntity);

		return modelBuilder;
	}
}
