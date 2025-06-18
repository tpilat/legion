using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL;

public class MessageArchiveConfiguration : IEntityTypeConfiguration<MessageBox.Model.MessageArchive>
{
	public const string PrimaryKeyFormatter = "{{\"IdMessage\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<MessageBox.Model.MessageArchive> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.MessageArchive> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdMessage);

		entityBuilder.ToTable("MessageArchive", "mbox");

		entityBuilder.HasIndex(e => e.IdMessageBoxInstance, "IXFK_MessageArchive_MessageBoxInstance");

		entityBuilder.HasIndex(e => e.IdMessageContent, "IXFK_MessageArchive_MessageContent");

		entityBuilder.HasIndex(e => e.IdMessageContent, "UQ_MessageArchive_IdMessageContent")
				.IsUnique();

		entityBuilder.HasIndex(e => e.IdMessageStatus, "IXFK_MessageArchive_MessageStatus");

		entityBuilder.HasIndex(e => e.IdMessageType, "IXFK_MessageArchive_MessageType");

		entityBuilder.HasIndex(e => e.IdQueue, "IXFK_MessageArchive_Queue");

		entityBuilder.HasIndex(e => e.IdTopic, "IXFK_MessageArchive_Topic");

		entityBuilder.Property(e => e.IdMessage)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdMessageType).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdMessageStatus).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdMessageContent).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdQueue).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdTopic).HasColumnType("uuid");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.MessageId)
			.HasColumnType("varchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.BusinessId)
			.HasColumnType("varchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.CorrelationId)
			.HasColumnType("varchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.SessionId).HasColumnType("uuid");

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uuid");

		entityBuilder.Property(e => e.Properties).HasColumnType("jsonb");

		entityBuilder.Property(e => e.Publisher)
			.HasColumnType("varchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.PublisherId)
			.HasColumnType("varchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.ValidToUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.IdMessageBoxInstance).HasColumnType("uuid");

		entityBuilder.HasOne(d => d.MessageBoxInstance)
			.WithMany(p => p.MessageArchives)
			.HasForeignKey(d => d.IdMessageBoxInstance)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_MessageArchive_MessageBoxInstance");

		entityBuilder.HasOne(d => d.MessageContent)
			.WithOne(p => p.MessageArchive)
			.HasForeignKey<MessageBox.Model.MessageArchive>(d => d.IdMessageContent)
			.HasConstraintName("FK_MessageArchive_IdMessageContent");

		entityBuilder.HasOne(d => d.MessageStatus)
			.WithMany(p => p.MessageArchives)
			.HasForeignKey(d => d.IdMessageStatus)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_MessageArchive_IdMessageStatus");

		entityBuilder.HasOne(d => d.MessageType)
			.WithMany(p => p.MessageArchives)
			.HasForeignKey(d => d.IdMessageType)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_MessageArchive_IdMessageType");

		entityBuilder.HasOne(d => d.Queue)
			.WithMany(p => p.MessageArchives)
			.HasForeignKey(d => d.IdQueue)
			.HasConstraintName("FK_MessageArchive_IdQueue");

		entityBuilder.HasOne(d => d.Topic)
			.WithMany(p => p.MessageArchives)
			.HasForeignKey(d => d.IdTopic)
			.HasConstraintName("FK_MessageArchive_IdTopic");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<MessageBox.Model.MessageArchive>(ConfigureEntity);

		return modelBuilder;
	}
}
