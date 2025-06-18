using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.Orchestrations.PostgreSQL;

public class OrchestrationConfiguration : IEntityTypeConfiguration<Orchestrations.Model.Orchestration>
{
	public const string PrimaryKeyFormatter = "{{\"IdOrchestration\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Orchestrations.Model.Orchestration> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Orchestrations.Model.Orchestration> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdOrchestration);

		entityBuilder.ToTable("Orchestration", "orch");

		entityBuilder.Property(e => e.IdOrchestration).ValueGeneratedNever();

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

		entityBuilder.Property(e => e.Version)
			.IsRequired()
			.HasMaxLength(31);

		entityBuilder.Property(e => e.ValidTo).HasColumnType("timestamp(6)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Orchestrations.Model.Orchestration>(ConfigureEntity);

		return modelBuilder;
	}
}
