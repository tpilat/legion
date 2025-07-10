using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.PostgreSQL;

public class OrchestrationStepConfiguration : IEntityTypeConfiguration<ServiceBus.Model.OrchestrationStep>
{
	public const string PrimaryKeyFormatter = "{{\"IdOrchestrationStep\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<ServiceBus.Model.OrchestrationStep> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<ServiceBus.Model.OrchestrationStep> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdOrchestrationStep);

		entityBuilder.ToTable("OrchestrationStep", "orch");

		entityBuilder.HasIndex(e => e.IdOrchestration, "IXFK_OrchestrationStep_Orchestration");

		entityBuilder.Property(e => e.IdOrchestrationStep)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdOrchestration).HasColumnType("uuid");

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

		entityBuilder.Property(e => e.Properties).HasColumnType("jsonb");

		entityBuilder.HasOne(d => d.Orchestration)
			.WithMany(p => p.OrchestrationSteps)
			.HasForeignKey(d => d.IdOrchestration)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OrchestrationStep_IdOrchestration");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<ServiceBus.Model.OrchestrationStep>(ConfigureEntity);

		return modelBuilder;
	}
}
