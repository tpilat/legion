using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL;

public class MessageConfiguration : IEntityTypeConfiguration<MessageBox.Model.Message>
{
	public const string PrimaryKeyFormatter = "{{\"IdMessage\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<MessageBox.Model.Message> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.Message> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdMessage);

		entityBuilder.ToTable("Message", "mbox");

		entityBuilder.HasIndex(e => e.IdMessageBoxInstance, "IXFK_Message_MessageBoxInstance");

		entityBuilder.HasIndex(e => e.IdMessageContent, "IXFK_Message_MessageContent");

		entityBuilder.HasIndex(e => e.IdMessageContent, "UQ_Message_IdMessageContent")
				.IsUnique();

		entityBuilder.HasIndex(e => e.IdMessageStatus, "IXFK_Message_MessageStatus");

		entityBuilder.HasIndex(e => e.IdMessageType, "IXFK_Message_MessageType");

		entityBuilder.HasIndex(e => e.IdQueue, "IXFK_Message_Queue");

		entityBuilder.HasIndex(e => e.IdTopic, "IXFK_Message_Topic");

		entityBuilder.Property(e => e.IdMessage)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdMessageType).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdMessageStatus).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdMessageContent).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdQueue).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdTopic).HasColumnType("uuid");

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

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.ValidToUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.IdMessageBoxInstance).HasColumnType("uuid");

		entityBuilder.HasOne(d => d.MessageBoxInstance)
			.WithMany(p => p.Messages)
			.HasForeignKey(d => d.IdMessageBoxInstance)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_Message_MessageBoxInstance");

		entityBuilder.HasOne(d => d.MessageContent)
			.WithOne(p => p.Message)
			.HasForeignKey<MessageBox.Model.Message>(d => d.IdMessageContent)
			.HasConstraintName("FK_Message_IdMessageContent");

		entityBuilder.HasOne(d => d.MessageStatus)
			.WithMany(p => p.Messages)
			.HasForeignKey(d => d.IdMessageStatus)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_Message_IdMessageStatus");

		entityBuilder.HasOne(d => d.MessageType)
			.WithMany(p => p.Messages)
			.HasForeignKey(d => d.IdMessageType)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_Message_IdMessageType");

		entityBuilder.HasOne(d => d.Queue)
			.WithMany(p => p.Messages)
			.HasForeignKey(d => d.IdQueue)
			.HasConstraintName("FK_Message_IdQueue");

		entityBuilder.HasOne(d => d.Topic)
			.WithMany(p => p.Messages)
			.HasForeignKey(d => d.IdTopic)
			.HasConstraintName("FK_Message_IdTopic");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<MessageBox.Model.Message>(ConfigureEntity);

		return modelBuilder;
	}
}
