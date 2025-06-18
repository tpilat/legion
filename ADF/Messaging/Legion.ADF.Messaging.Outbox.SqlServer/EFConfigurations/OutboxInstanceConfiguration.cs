using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Outbox.SqlServer;

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
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("nvarchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.Version)
			.IsRequired()
			.HasColumnType("nvarchar(15)")
			.HasMaxLength(15);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Outbox.Model.OutboxInstance>(ConfigureEntity);

		return modelBuilder;
	}
}
