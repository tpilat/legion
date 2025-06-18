using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.Orchestrations.PostgreSQL;

public class OrchestrationStepConfiguration : IEntityTypeConfiguration<Orchestrations.Model.OrchestrationStep>
{
	public const string PrimaryKeyFormatter = "{{\"IdOrchestrationStep\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Orchestrations.Model.OrchestrationStep> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Orchestrations.Model.OrchestrationStep> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdOrchestrationStep);

		entityBuilder.ToTable("OrchestrationStep", "orch");

		entityBuilder.HasIndex(e => e.IdOrchestration, "IXFK_OrchestrationStep_IdOrchestration");

		entityBuilder.Property(e => e.IdOrchestrationStep).ValueGeneratedNever();

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasMaxLength(127);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasMaxLength(127);

		entityBuilder.Property(e => e.Description).HasMaxLength(1023);

		entityBuilder.Property(e => e.Class)
			.IsRequired()
			.HasMaxLength(2047);

		entityBuilder.Property(e => e.Properties).HasColumnType("jsonb");

		entityBuilder.HasOne(d => d.Orchestration)
			.WithMany(p => p.OrchestrationSteps)
			.HasForeignKey(d => d.IdOrchestration)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_OrchestrationStep_IdOrchestration");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Orchestrations.Model.OrchestrationStep>(ConfigureEntity);

		return modelBuilder;
	}
}
