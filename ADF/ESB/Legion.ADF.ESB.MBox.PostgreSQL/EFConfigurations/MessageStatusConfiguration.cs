using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.MBox.PostgreSQL;

public class MessageStatusConfiguration : IEntityTypeConfiguration<MBox.Model.MessageStatus>
{
	public const string PrimaryKeyFormatter = "{{\"IdMessageStatus\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<MBox.Model.MessageStatus> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MBox.Model.MessageStatus> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdMessageStatus);

		entityBuilder.ToTable("MessageStatus", "mbox");

		entityBuilder.Property(e => e.IdMessageStatus).ValueGeneratedNever();

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasMaxLength(63);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasMaxLength(127);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<MBox.Model.MessageStatus>(ConfigureEntity);

		return modelBuilder;
	}
}
