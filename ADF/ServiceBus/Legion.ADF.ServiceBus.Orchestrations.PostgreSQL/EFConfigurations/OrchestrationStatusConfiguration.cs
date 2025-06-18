using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.Orchestrations.PostgreSQL;

public class OrchestrationStatusConfiguration : IEntityTypeConfiguration<Orchestrations.Model.OrchestrationStatus>
{
	public const string PrimaryKeyFormatter = "{{\"IdOrchestrationStatus\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Orchestrations.Model.OrchestrationStatus> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Orchestrations.Model.OrchestrationStatus> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdOrchestrationStatus);

		entityBuilder.ToTable("OrchestrationStatus", "orch");

		entityBuilder.Property(e => e.IdOrchestrationStatus)
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
		modelBuilder.Entity<Orchestrations.Model.OrchestrationStatus>(ConfigureEntity);

		return modelBuilder;
	}
}
