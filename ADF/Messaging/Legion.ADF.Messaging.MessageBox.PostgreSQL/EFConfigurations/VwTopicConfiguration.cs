using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL;

public class VwTopicConfiguration : IEntityTypeConfiguration<MessageBox.Model.VwTopic>
{
	public void Configure(EntityTypeBuilder<MessageBox.Model.VwTopic> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.VwTopic> entityBuilder)
	{
		entityBuilder.ToView("VwTopic", "mbox")
			.HasNoKey();

		entityBuilder.Property(e => e.IdTopic).HasColumnType("uuid");

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("varchar(1023)");

		entityBuilder.Property(e => e.TimeoutForMessageProcessing).HasColumnType("interval");

		entityBuilder.Property(e => e.Properties).HasColumnType("jsonb");

		entityBuilder.Property(e => e.IdProcessingMode).HasColumnType("uuid");

		entityBuilder.Property(e => e.ProcessingModeCode)
			.IsRequired()
			.HasColumnType("varchar(63)");

		entityBuilder.Property(e => e.ProcessingModeName)
			.IsRequired()
			.HasColumnType("varchar(127)");

		entityBuilder.Property(e => e.IdSuspendingMode).HasColumnType("uuid");

		entityBuilder.Property(e => e.SuspendingModeCode)
			.IsRequired()
			.HasColumnType("varchar(63)");

		entityBuilder.Property(e => e.SuspendingModeName)
			.IsRequired()
			.HasColumnType("varchar(127)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<MessageBox.Model.VwTopic>(ConfigureEntity);
}
