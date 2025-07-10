using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.SqlServer;

public class OrchestrationStatusConfiguration : IEntityTypeConfiguration<ServiceBus.Model.OrchestrationStatus>
{
	public const string PrimaryKeyFormatter = "{{\"IdOrchestrationStatus\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<ServiceBus.Model.OrchestrationStatus> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<ServiceBus.Model.OrchestrationStatus> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdOrchestrationStatus);

		entityBuilder.ToTable("OrchestrationStatus", "orch");

		entityBuilder.Property(e => e.IdOrchestrationStatus)
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
		modelBuilder.Entity<ServiceBus.Model.OrchestrationStatus>(ConfigureEntity);

		return modelBuilder;
	}
}
