using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.MBox.PostgreSQL;

public class MessageConfiguration : IEntityTypeConfiguration<MBox.Model.Message>
{
	public const string PrimaryKeyFormatter = "{{\"IdMessage\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<MBox.Model.Message> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MBox.Model.Message> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdMessage);

		entityBuilder.ToTable("Message", "mbox");

		entityBuilder.HasIndex(e => e.BusinessId, "IX_Message_BusinessId");

		entityBuilder.HasIndex(e => e.IdMessageContent, "IXFK_Message_IdMessageContent");

		entityBuilder.HasIndex(e => e.IdMessageContent, "UQ_Message_IdMessageContent")
				.IsUnique();

		entityBuilder.HasIndex(e => e.IdMessageStatus, "IXFK_Message_IdMessageStatus");

		entityBuilder.HasIndex(e => e.IdMessageType, "IXFK_Message_IdMessageType");

		entityBuilder.HasIndex(e => e.IdPreviousMessage, "IXFK_Message_IdPreviousMessage");

		entityBuilder.Property(e => e.IdMessage).ValueGeneratedNever();

		entityBuilder.Property(e => e.SelfProperties).HasColumnType("jsonb");

		entityBuilder.Property(e => e.ContextProperties).HasColumnType("jsonb");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamp(6)");

		entityBuilder.Property(e => e.ExternalId).HasMaxLength(511);

		entityBuilder.Property(e => e.ValidToUtc).HasColumnType("timestamp(6)");

		entityBuilder.HasOne(d => d.MessageContent)
			.WithOne(p => p.Message)
			.HasForeignKey<MBox.Model.Message>(d => d.IdMessageContent)
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

		entityBuilder.HasOne(d => d.PreviousMessage)
			.WithMany(p => p.NextMessages)
			.HasForeignKey(d => d.IdPreviousMessage)
			.HasConstraintName("FK_Message_IdPreviousMessage");

		entityBuilder.HasMany(d => d.MessagePublishings)
			.WithOne()
			.HasForeignKey(d => d.IdAdapter)
			.HasConstraintName("FK_MessagePublishing_IdAdapter");

		entityBuilder.HasMany(d => d.MessagePublishings)
			.WithOne()
			.HasForeignKey(d => d.IdJob)
			.HasConstraintName("FK_MessagePublishing_IdJob");

		entityBuilder.HasMany(d => d.MessagePublishings)
			.WithOne()
			.HasForeignKey(d => d.IdStepInstance)
			.HasConstraintName("FK_MessagePublishing_IdStepInstance");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<MBox.Model.Message>(ConfigureEntity);

		return modelBuilder;
	}
}
