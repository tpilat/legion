using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.MBox.PostgreSQL;

public class MessageProcessingStatusConfiguration : IEntityTypeConfiguration<MBox.Model.MessageProcessingStatus>
{
	public const string PrimaryKeyFormatter = "{{\"IdMessageProcessingStatus\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<MBox.Model.MessageProcessingStatus> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MBox.Model.MessageProcessingStatus> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdMessageProcessingStatus);

		entityBuilder.ToTable("MessageProcessingStatus", "mbox");

		entityBuilder.Property(e => e.IdMessageProcessingStatus).ValueGeneratedNever();

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasMaxLength(63);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasMaxLength(63);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<MBox.Model.MessageProcessingStatus>(ConfigureEntity);

		return modelBuilder;
	}
}
