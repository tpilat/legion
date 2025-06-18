using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.SqlServer;

public class TopicConfiguration : IEntityTypeConfiguration<MessageBox.Model.Topic>
{
	public const string PrimaryKeyFormatter = "{{\"IdTopic\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<MessageBox.Model.Topic> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.Topic> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdTopic);

		entityBuilder.ToTable("Topic", "mbox");

		entityBuilder.HasIndex(e => e.IdMessageBoxInstance, "IXFK_Topic_MessageBoxInstance");

		entityBuilder.HasIndex(e => e.IdProcessingMode, "IXFK_Topic_QueueProcessingMode");

		entityBuilder.HasIndex(e => e.IdSuspendingMode, "IXFK_Topic_QueueProcessingMode_02");

		entityBuilder.HasIndex(e => e.Name, "UQ_Topic_Name")
				.IsUnique();

		entityBuilder.Property(e => e.IdTopic)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("nvarchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.IsActive).HasColumnType("bit");

		entityBuilder.Property(e => e.IsSequentialFIFO).HasColumnType("bit");

		entityBuilder.Property(e => e.TimeoutForMessageProcessing).HasColumnType("time(7)");

		entityBuilder.Property(e => e.Properties).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.IdProcessingMode).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdSuspendingMode).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdMessageBoxInstance).HasColumnType("uniqueidentifier");

		entityBuilder.HasOne(d => d.MessageBoxInstance)
			.WithMany(p => p.Topics)
			.HasForeignKey(d => d.IdMessageBoxInstance)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_Topic_MessageBoxInstance");

		entityBuilder.HasOne(d => d.ProcessingMode)
			.WithMany(p => p.Topics)
			.HasForeignKey(d => d.IdProcessingMode)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_Topic_IdProcessingMode");

		entityBuilder.HasOne(d => d.SuspendingMode)
			.WithMany(p => p.SuspendingModeTopics)
			.HasForeignKey(d => d.IdSuspendingMode)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_Topic_IdSuspendingMode");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<MessageBox.Model.Topic>(ConfigureEntity);

		return modelBuilder;
	}
}
