using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL;

public class MessageProcessingLogConfiguration : IEntityTypeConfiguration<MessageBox.Model.MessageProcessingLog>
{
	public const string PrimaryKeyFormatter = "{{\"IdMessageProcessingLog\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<MessageBox.Model.MessageProcessingLog> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.MessageProcessingLog> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdMessageProcessingLog);

		entityBuilder.ToTable("MessageProcessingLog", "mbox");

		entityBuilder.HasIndex(e => e.IdMessage, "IX_MessageProcessingLog_IdMessage");

		entityBuilder.HasIndex(e => e.IdMessageBoxInstance, "IXFK_MessageProcessingLog_MessageBoxInstance");

		entityBuilder.HasIndex(e => e.IdMessageProcessingStatus, "IXFK_MessageProcessingLog_MessageProcessingStatus");

		entityBuilder.HasIndex(e => e.IdQueuedMessage, "IXFK_MessageProcessingLog_QueuedMessage");

		entityBuilder.HasIndex(e => e.IdSubscribedMessage, "IXFK_MessageProcessingLog_SubscribedMessage");

		entityBuilder.Property(e => e.IdMessageProcessingLog)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdMessage).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdQueuedMessage).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdSubscribedMessage).HasColumnType("uuid");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.IdMessageProcessingStatus).HasColumnType("uuid");

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdLogMessage).HasColumnType("uuid");

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("varchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.IdMessageBoxInstance).HasColumnType("uuid");

		entityBuilder.HasOne(d => d.MessageBoxInstance)
			.WithMany(p => p.MessageProcessingLogs)
			.HasForeignKey(d => d.IdMessageBoxInstance)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_MessageProcessingLog_MessageBoxInstance");

		entityBuilder.HasOne(d => d.MessageProcessingStatus)
			.WithMany(p => p.MessageProcessingLogs)
			.HasForeignKey(d => d.IdMessageProcessingStatus)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_MessageProcessingLog_IdMessageProcessingStatus");

		entityBuilder.HasOne(d => d.QueuedMessage)
			.WithMany(p => p.MessageProcessingLogs)
			.HasForeignKey(d => d.IdQueuedMessage)
			.HasConstraintName("FK_MessageProcessingLog_IdQueuedMessage");

		entityBuilder.HasOne(d => d.SubscribedMessage)
			.WithMany(p => p.MessageProcessingLogs)
			.HasForeignKey(d => d.IdSubscribedMessage)
			.HasConstraintName("FK_MessageProcessingLog_IdSubscribedMessage");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<MessageBox.Model.MessageProcessingLog>(ConfigureEntity);

		return modelBuilder;
	}
}
