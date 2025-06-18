using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.SqlServer;

public class MessageBoxProcessingLogConfiguration : IEntityTypeConfiguration<MessageBox.Model.MessageBoxProcessingLog>
{
	public const string PrimaryKeyFormatter = "{{\"IdMessageBoxProcessingLog\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<MessageBox.Model.MessageBoxProcessingLog> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.MessageBoxProcessingLog> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdMessageBoxProcessingLog);

		entityBuilder.ToTable("MessageBoxProcessingLog", "mbox");

		entityBuilder.HasIndex(e => e.IdMessageBoxInstance, "IXFK_MessageBoxProcessingLog_MessageBoxInstance");

		entityBuilder.HasIndex(e => e.IdQueue, "IXFK_MessageBoxProcessingLog_Queue");

		entityBuilder.HasIndex(e => e.IdTopic, "IXFK_MessageBoxProcessingLog_Topic");

		entityBuilder.HasIndex(e => e.IdTopicSubscription, "IXFK_MessageBoxProcessingLog_TopicSubscription");

		entityBuilder.Property(e => e.IdMessageBoxProcessingLog)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdMessageBoxInstance).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdQueue).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdTopic).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdTopicSubscription).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdLogMessage).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("nvarchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.Detail).HasColumnType("nvarchar(max)");

		entityBuilder.HasOne(d => d.MessageBoxInstance)
			.WithMany(p => p.MessageBoxProcessingLogs)
			.HasForeignKey(d => d.IdMessageBoxInstance)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_MessageBoxProcessingLog_MessageBoxInstance");

		entityBuilder.HasOne(d => d.Queue)
			.WithMany(p => p.MessageBoxProcessingLogs)
			.HasForeignKey(d => d.IdQueue)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_MessageBoxProcessingLog_Queue");

		entityBuilder.HasOne(d => d.Topic)
			.WithMany(p => p.MessageBoxProcessingLogs)
			.HasForeignKey(d => d.IdTopic)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_MessageBoxProcessingLog_Topic");

		entityBuilder.HasOne(d => d.TopicSubscription)
			.WithMany(p => p.MessageBoxProcessingLogs)
			.HasForeignKey(d => d.IdTopicSubscription)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_MessageBoxProcessingLog_TopicSubscription");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<MessageBox.Model.MessageBoxProcessingLog>(ConfigureEntity);

		return modelBuilder;
	}
}
