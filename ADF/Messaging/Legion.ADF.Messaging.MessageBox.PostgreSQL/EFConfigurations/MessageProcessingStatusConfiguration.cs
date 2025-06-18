using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL;

public class MessageProcessingStatusConfiguration : IEntityTypeConfiguration<MessageBox.Model.MessageProcessingStatus>
{
	public const string PrimaryKeyFormatter = "{{\"IdMessageProcessingStatus\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<MessageBox.Model.MessageProcessingStatus> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.MessageProcessingStatus> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdMessageProcessingStatus);

		entityBuilder.ToTable("MessageProcessingStatus", "mbox");

		entityBuilder.Property(e => e.IdMessageProcessingStatus)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("varchar(63)")
			.HasMaxLength(63);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("varchar(127)")
			.HasMaxLength(127);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<MessageBox.Model.MessageProcessingStatus>(ConfigureEntity);

		return modelBuilder;
	}
}
