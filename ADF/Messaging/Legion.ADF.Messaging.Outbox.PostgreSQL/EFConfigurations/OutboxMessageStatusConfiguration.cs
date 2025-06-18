using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Outbox.PostgreSQL;

public class OutboxMessageStatusConfiguration : IEntityTypeConfiguration<Outbox.Model.OutboxMessageStatus>
{
	public const string PrimaryKeyFormatter = "{{\"IdOutboxMessageStatus\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Outbox.Model.OutboxMessageStatus> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Outbox.Model.OutboxMessageStatus> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdOutboxMessageStatus);

		entityBuilder.ToTable("OutboxMessageStatus", "outbox");

		entityBuilder.Property(e => e.IdOutboxMessageStatus)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("varchar(63)")
			.HasMaxLength(63);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("varchar(127)")
			.HasMaxLength(127);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Outbox.Model.OutboxMessageStatus>(ConfigureEntity);

		return modelBuilder;
	}
}
