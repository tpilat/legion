using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.MBox.PostgreSQL;

public class QueueConfiguration : IEntityTypeConfiguration<MBox.Model.Queue>
{
	public const string PrimaryKeyFormatter = "{{\"IdQueue\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<MBox.Model.Queue> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MBox.Model.Queue> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdQueue);

		entityBuilder.ToTable("Queue", "mbox");

		entityBuilder.HasIndex(e => e.IdAdapter, "IXFK_Queue_IdAdapter");

		entityBuilder.HasIndex(e => e.IdJob, "IXFK_Queue_IdJob");

		entityBuilder.HasIndex(e => e.IdMessageType, "IXFK_Queue_IdMessageType");

		entityBuilder.HasIndex(e => e.IdOrchestration, "IXFK_Queue_IdOrchestration");

		entityBuilder.Property(e => e.IdQueue).ValueGeneratedNever();

		entityBuilder.Property(e => e.Properties).HasColumnType("jsonb");

		entityBuilder.HasOne(d => d.MessageType)
			.WithMany(p => p.Queues)
			.HasForeignKey(d => d.IdMessageType)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_Queue_IdMessageType");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<MBox.Model.Queue>(ConfigureEntity);

		return modelBuilder;
	}
}
