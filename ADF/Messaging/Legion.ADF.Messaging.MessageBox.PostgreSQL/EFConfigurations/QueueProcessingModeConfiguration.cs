using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL;

public class QueueProcessingModeConfiguration : IEntityTypeConfiguration<MessageBox.Model.QueueProcessingMode>
{
	public const string PrimaryKeyFormatter = "{{\"IdQueueProcessingMode\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<MessageBox.Model.QueueProcessingMode> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.QueueProcessingMode> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdQueueProcessingMode);

		entityBuilder.ToTable("QueueProcessingMode", "mbox");

		entityBuilder.Property(e => e.IdQueueProcessingMode)
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
		modelBuilder.Entity<MessageBox.Model.QueueProcessingMode>(ConfigureEntity);

		return modelBuilder;
	}
}
