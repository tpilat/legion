using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Outbox.PostgreSQL;

public class OutboxInstanceConfiguration : IEntityTypeConfiguration<Outbox.Model.OutboxInstance>
{
	public const string PrimaryKeyFormatter = "{{\"IdOutboxInstance\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Outbox.Model.OutboxInstance> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Outbox.Model.OutboxInstance> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdOutboxInstance);

		entityBuilder.ToTable("OutboxInstance", "outbox");

		entityBuilder.Property(e => e.IdOutboxInstance)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("varchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.Version)
			.IsRequired()
			.HasColumnType("varchar(15)")
			.HasMaxLength(15);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Outbox.Model.OutboxInstance>(ConfigureEntity);

		return modelBuilder;
	}
}
