using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.PostgreSQL;

public class OrchestrationConfiguration : IEntityTypeConfiguration<ServiceBus.Model.Orchestration>
{
	public const string PrimaryKeyFormatter = "{{\"IdOrchestration\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<ServiceBus.Model.Orchestration> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<ServiceBus.Model.Orchestration> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdOrchestration);

		entityBuilder.ToTable("Orchestration", "orch");

		entityBuilder.Property(e => e.IdOrchestration)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("varchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.Description)
			.HasColumnType("varchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.Namespace)
			.IsRequired()
			.HasColumnType("varchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.Version)
			.IsRequired()
			.HasColumnType("varchar(31)")
			.HasMaxLength(31);

		entityBuilder.Property(e => e.Properties).HasColumnType("jsonb");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<ServiceBus.Model.Orchestration>(ConfigureEntity);

		return modelBuilder;
	}
}
