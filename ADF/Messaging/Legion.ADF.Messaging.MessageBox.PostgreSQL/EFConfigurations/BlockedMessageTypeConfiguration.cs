using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL;

public class BlockedMessageTypeConfiguration : IEntityTypeConfiguration<MessageBox.Model.BlockedMessageType>
{
	public const string PrimaryKeyFormatter = "{{\"IdBlockedMessageType\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<MessageBox.Model.BlockedMessageType> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.BlockedMessageType> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdBlockedMessageType);

		entityBuilder.ToTable("BlockedMessageType", "mbox");

		entityBuilder.HasIndex(e => e.IdMessageBoxInstance, "IXFK_BlockedMessageType_MessageBoxInstance");

		entityBuilder.HasIndex(e => e.Namespace, "UQ_BlockedMessageType_Namespace")
				.IsUnique();

		entityBuilder.Property(e => e.IdBlockedMessageType)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Namespace)
			.IsRequired()
			.HasColumnType("varchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.IdMessageBoxInstance).HasColumnType("uuid");

		entityBuilder.HasOne(d => d.MessageBoxInstance)
			.WithMany(p => p.BlockedMessageTypes)
			.HasForeignKey(d => d.IdMessageBoxInstance)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_BlockedMessageType_MessageBoxInstance");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<MessageBox.Model.BlockedMessageType>(ConfigureEntity);

		return modelBuilder;
	}
}
