using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.MBox.PostgreSQL;

public class VwQueuedMessageConfiguration : IEntityTypeConfiguration<MBox.Model.VwQueuedMessage>
{
	public void Configure(EntityTypeBuilder<MBox.Model.VwQueuedMessage> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MBox.Model.VwQueuedMessage> entityBuilder)
	{
		entityBuilder.ToView("VwQueuedMessage", "mbox")
			.HasNoKey();

		entityBuilder.Property(e => e.QueuedUtc).HasColumnType("timestamp(6)");

		entityBuilder.Property(e => e.LastProcessedUtc).HasColumnType("timestamp(6)");

		entityBuilder.Property(e => e.NextProcessingUtc).HasColumnType("timestamp(6)");

		entityBuilder.Property(e => e.ProcessedUtc).HasColumnType("timestamp(6)");

		entityBuilder.Property(e => e.TerminatedUtc).HasColumnType("timestamp(6)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<MBox.Model.VwQueuedMessage>(ConfigureEntity);
}
