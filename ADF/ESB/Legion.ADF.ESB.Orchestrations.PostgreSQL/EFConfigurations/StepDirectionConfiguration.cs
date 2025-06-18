using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.Orchestrations.PostgreSQL;

public class StepDirectionConfiguration : IEntityTypeConfiguration<Orchestrations.Model.StepDirection>
{
	public const string PrimaryKeyFormatter = "{{\"IdStepDirection\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Orchestrations.Model.StepDirection> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Orchestrations.Model.StepDirection> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdStepDirection);

		entityBuilder.ToTable("StepDirection", "orch");

		entityBuilder.HasIndex(e => e.IdFromStep, "IXFK_StepDirection_IdFromStep");

		entityBuilder.HasIndex(e => e.IdToStep, "IXFK_StepDirection_IdToStep");

		entityBuilder.Property(e => e.IdStepDirection).ValueGeneratedNever();

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamp(6)");

		entityBuilder.HasOne(d => d.FromStep)
			.WithMany(p => p.StepDirections)
			.HasForeignKey(d => d.IdFromStep)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_StepDirection_IdFromStep");

		entityBuilder.HasOne(d => d.ToStep)
			.WithMany(p => p.ToStepStepDirections)
			.HasForeignKey(d => d.IdToStep)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_StepDirection_IdToStep");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Orchestrations.Model.StepDirection>(ConfigureEntity);

		return modelBuilder;
	}
}
