using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.Components.PostgreSQL;

public class AdapterConfiguration : IEntityTypeConfiguration<Components.Model.Adapter>
{
	public const string PrimaryKeyFormatter = "{{\"IdAdapter\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Components.Model.Adapter> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Components.Model.Adapter> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdAdapter);

		entityBuilder.ToTable("Adapter", "comp");

		entityBuilder.HasIndex(e => e.IdAdapterStatus, "IXFK_Adapter_IdAdapterStatus");

		entityBuilder.Property(e => e.IdAdapter).ValueGeneratedNever();

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

		entityBuilder.HasOne(d => d.AdapterStatus)
			.WithMany(p => p.Adapters)
			.HasForeignKey(d => d.IdAdapterStatus)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_Adapter_IdAdapterStatus");

		entityBuilder.HasMany(d => d.AdapterLogs)
			.WithOne()
			.HasForeignKey(d => d.IdMessageProcessingLog)
			.HasConstraintName("FK_AdapterLog_IdMessageProcessingLog");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Components.Model.Adapter>(ConfigureEntity);

		return modelBuilder;
	}
}
