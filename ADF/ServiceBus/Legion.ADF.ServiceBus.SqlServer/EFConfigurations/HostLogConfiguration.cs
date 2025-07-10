using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.SqlServer;

public class HostLogConfiguration : IEntityTypeConfiguration<ServiceBus.Model.HostLog>
{
	public const string PrimaryKeyFormatter = "{{\"IdHostLog\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<ServiceBus.Model.HostLog> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<ServiceBus.Model.HostLog> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdHostLog);

		entityBuilder.ToTable("HostLog", "hosts");

		entityBuilder.HasIndex(e => e.IdHost, "IXFK_HostLog_Host");

		entityBuilder.Property(e => e.IdHostLog)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdHost).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.IsRunning).HasColumnType("bit");

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdLogMessage).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("nvarchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.Detail).HasColumnType("nvarchar(max)");

		entityBuilder.HasOne(d => d.Host)
			.WithMany(p => p.HostLogs)
			.HasForeignKey(d => d.IdHost)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_HostLog_IdHost");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<ServiceBus.Model.HostLog>(ConfigureEntity);

		return modelBuilder;
	}
}
