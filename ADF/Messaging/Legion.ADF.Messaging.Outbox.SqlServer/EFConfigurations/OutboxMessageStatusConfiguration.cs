using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Outbox.SqlServer;

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
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("nvarchar(63)")
			.HasMaxLength(63);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("nvarchar(127)")
			.HasMaxLength(127);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Outbox.Model.OutboxMessageStatus>(ConfigureEntity);

		return modelBuilder;
	}
}
