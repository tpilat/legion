using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.Components.PostgreSQL;

public class AdapterStatusConfiguration : IEntityTypeConfiguration<Components.Model.AdapterStatus>
{
	public const string PrimaryKeyFormatter = "{{\"IdAdapterStatus\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Components.Model.AdapterStatus> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Components.Model.AdapterStatus> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdAdapterStatus);

		entityBuilder.ToTable("AdapterStatus", "comp");

		entityBuilder.Property(e => e.IdAdapterStatus).ValueGeneratedNever();

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasMaxLength(63);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasMaxLength(127);

		entityBuilder.HasMany(d => d.AdapterLogs)
			.WithOne()
			.HasForeignKey(d => d.IdMessageProcessingLog)
			.HasConstraintName("FK_AdapterLog_IdMessageProcessingLog");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Components.Model.AdapterStatus>(ConfigureEntity);

		return modelBuilder;
	}
}
