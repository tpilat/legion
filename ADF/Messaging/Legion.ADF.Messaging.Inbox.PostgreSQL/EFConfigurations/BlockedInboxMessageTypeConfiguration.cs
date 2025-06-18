using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Inbox.PostgreSQL;

public class BlockedInboxMessageTypeConfiguration : IEntityTypeConfiguration<Inbox.Model.BlockedInboxMessageType>
{
	public const string PrimaryKeyFormatter = "{{\"IdBlockedInboxMessageType\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Inbox.Model.BlockedInboxMessageType> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Inbox.Model.BlockedInboxMessageType> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdBlockedInboxMessageType);

		entityBuilder.ToTable("BlockedInboxMessageType", "inbox");

		entityBuilder.HasIndex(e => e.IdInboxInstance, "IXFK_BlockedInboxMessageType_InboxInstance");

		entityBuilder.HasIndex(e => e.Namespace, "UQ_BlockedInboxMessageType_Namespace")
				.IsUnique();

		entityBuilder.Property(e => e.IdBlockedInboxMessageType)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Namespace)
			.IsRequired()
			.HasColumnType("varchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.IdInboxInstance).HasColumnType("uuid");

		entityBuilder.HasOne(d => d.InboxInstance)
			.WithMany(p => p.BlockedInboxMessageTypes)
			.HasForeignKey(d => d.IdInboxInstance)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_BlockedInboxMessageType_InboxInstance");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Inbox.Model.BlockedInboxMessageType>(ConfigureEntity);

		return modelBuilder;
	}
}
