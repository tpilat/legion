using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Outbox.SqlServer;

public class BlockedOutboxMessageTypeConfiguration : IEntityTypeConfiguration<Outbox.Model.BlockedOutboxMessageType>
{
	public const string PrimaryKeyFormatter = "{{\"IdBlockedOutboxMessageType\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Outbox.Model.BlockedOutboxMessageType> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Outbox.Model.BlockedOutboxMessageType> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdBlockedOutboxMessageType);

		entityBuilder.ToTable("BlockedOutboxMessageType", "outbox");

		entityBuilder.HasIndex(e => e.IdOutboxInstance, "IXFK_BlockedOutboxMessageType_OutboxInstance");

		entityBuilder.HasIndex(e => e.Namespace, "UQ_BlockedOutboxMessageType_Namespace")
				.IsUnique();

		entityBuilder.Property(e => e.IdBlockedOutboxMessageType)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Namespace)
			.IsRequired()
			.HasColumnType("nvarchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.IdOutboxInstance).HasColumnType("uniqueidentifier");

		entityBuilder.HasOne(d => d.OutboxInstance)
			.WithMany(p => p.BlockedOutboxMessageTypes)
			.HasForeignKey(d => d.IdOutboxInstance)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_BlockedOutboxMessageType_OutboxInstance");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Outbox.Model.BlockedOutboxMessageType>(ConfigureEntity);

		return modelBuilder;
	}
}
