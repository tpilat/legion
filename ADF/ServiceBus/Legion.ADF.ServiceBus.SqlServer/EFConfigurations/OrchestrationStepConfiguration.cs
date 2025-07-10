using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.SqlServer;

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
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdOrchestration).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IsMainEntry).HasColumnType("bit");

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("nvarchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.Description)
			.HasColumnType("nvarchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.Namespace)
			.IsRequired()
			.HasColumnType("nvarchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.Properties).HasColumnType("nvarchar(max)");

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
