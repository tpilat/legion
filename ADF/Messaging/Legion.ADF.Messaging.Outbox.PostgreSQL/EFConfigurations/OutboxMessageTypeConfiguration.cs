using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Outbox.PostgreSQL;

public class OutboxMessageTypeConfiguration : IEntityTypeConfiguration<Outbox.Model.OutboxMessageType>
{
	public const string PrimaryKeyFormatter = "{{\"IdOutboxMessageType\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Outbox.Model.OutboxMessageType> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Outbox.Model.OutboxMessageType> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdOutboxMessageType);

		entityBuilder.ToTable("OutboxMessageType", "outbox");

		entityBuilder.HasIndex(e => e.IdOutboxInstance, "IXFK_OutboxMessageType_OutboxInstance");

		entityBuilder.HasIndex(e => e.Namespace, "UQ_OutboxMessageType_Namespace")
				.IsUnique();

		entityBuilder.Property(e => e.IdOutboxMessageType)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("varchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("varchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.Namespace)
			.IsRequired()
			.HasColumnType("varchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.IdOutboxInstance).HasColumnType("uuid");

		entityBuilder.HasOne(d => d.OutboxInstance)
			.WithMany(p => p.OutboxMessageTypes)
			.HasForeignKey(d => d.IdOutboxInstance)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OutboxMessageType_IdOutboxInstance");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Outbox.Model.OutboxMessageType>(ConfigureEntity);

		return modelBuilder;
	}
}
