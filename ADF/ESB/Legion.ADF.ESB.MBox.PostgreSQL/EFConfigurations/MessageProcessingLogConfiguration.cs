using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.MBox.PostgreSQL;

public class MessageProcessingLogConfiguration : IEntityTypeConfiguration<MBox.Model.MessageProcessingLog>
{
	public const string PrimaryKeyFormatter = "{{\"IdMessageProcessingLog\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<MBox.Model.MessageProcessingLog> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MBox.Model.MessageProcessingLog> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdMessageProcessingLog);

		entityBuilder.ToTable("MessageProcessingLog", "mbox");

		entityBuilder.HasIndex(e => e.IdMessageProcessingStatus, "IXFK_MessageProcessingLog_IdMessageProcessingStatus");

		entityBuilder.HasIndex(e => e.IdQueuedMessage, "IXFK_MessageProcessingLog_IdQueuedMessage");

		entityBuilder.Property(e => e.IdMessageProcessingLog).ValueGeneratedNever();

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamp(6)");

		entityBuilder.Property(e => e.Detail).IsRequired();

		entityBuilder.Property(e => e.Data).HasColumnType("jsonb");

		entityBuilder.HasOne(d => d.MessageProcessingStatus)
			.WithMany(p => p.MessageProcessingLogs)
			.HasForeignKey(d => d.IdMessageProcessingStatus)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_MessageProcessingLog_IdMessageProcessingStatus");

		entityBuilder.HasOne(d => d.QueuedMessage)
			.WithMany(p => p.MessageProcessingLogs)
			.HasForeignKey(d => d.IdQueuedMessage)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_MessageProcessingLog_IdQueuedMessage");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<MBox.Model.MessageProcessingLog>(ConfigureEntity);

		return modelBuilder;
	}
}
