using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.Components.PostgreSQL;

public class AdapterLogConfiguration : IEntityTypeConfiguration<Components.Model.AdapterLog>
{
	public const string PrimaryKeyFormatter = "{{\"IdAdapterLog\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Components.Model.AdapterLog> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Components.Model.AdapterLog> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdAdapterLog);

		entityBuilder.ToTable("AdapterLog", "comp");

		entityBuilder.HasIndex(e => e.IdAdapter, "IXFK_AdapterLog_IdAdapter");

		entityBuilder.HasIndex(e => e.IdAdapterStatus, "IXFK_AdapterLog_IdAdapterStatus");

		entityBuilder.HasIndex(e => e.IdMessageProcessingLog, "IXFK_AdapterLog_IdMessageProcessingLog");

		entityBuilder.Property(e => e.IdAdapterLog).ValueGeneratedNever();

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamp(6)");

		entityBuilder.Property(e => e.Detail).IsRequired();

		entityBuilder.Property(e => e.Data).HasColumnType("jsonb");

		entityBuilder.HasOne(d => d.Adapter)
			.WithMany(p => p.AdapterLogs)
			.HasForeignKey(d => d.IdAdapter)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_AdapterLog_IdAdapter");

		entityBuilder.HasOne(d => d.AdapterStatus)
			.WithMany(p => p.AdapterLogs)
			.HasForeignKey(d => d.IdAdapterStatus)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_AdapterLog_IdAdapterStatus");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Components.Model.AdapterLog>(ConfigureEntity);

		return modelBuilder;
	}
}
