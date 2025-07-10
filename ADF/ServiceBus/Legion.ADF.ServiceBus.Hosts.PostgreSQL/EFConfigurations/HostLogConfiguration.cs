using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.Hosts.PostgreSQL;

public class HostLogConfiguration : IEntityTypeConfiguration<Hosts.Model.HostLog>
{
	public const string PrimaryKeyFormatter = "{{\"IdHostLog\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Hosts.Model.HostLog> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Hosts.Model.HostLog> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdHostLog);

		entityBuilder.ToTable("HostLog", "hosts");

		entityBuilder.HasIndex(e => e.IdHost, "IXFK_HostLog_Host");

		entityBuilder.Property(e => e.IdHostLog)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdHost).HasColumnType("uuid");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdLogMessage).HasColumnType("uuid");

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("varchar(127)")
			.HasMaxLength(127);

		entityBuilder.HasOne(d => d.Host)
			.WithMany(p => p.HostLogs)
			.HasForeignKey(d => d.IdHost)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_HostLog_IdHost");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Hosts.Model.HostLog>(ConfigureEntity);

		return modelBuilder;
	}
}
