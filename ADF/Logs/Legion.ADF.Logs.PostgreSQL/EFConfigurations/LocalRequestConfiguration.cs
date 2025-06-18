using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Logs.PostgreSQL;

public class LocalRequestConfiguration : IEntityTypeConfiguration<Logs.Model.LocalRequest>
{
	public const string PrimaryKeyFormatter = "{{\"IdLocalRequest\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Logs.Model.LocalRequest> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Logs.Model.LocalRequest> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdLocalRequest);

		entityBuilder.ToTable("LocalRequest", "log");

		entityBuilder.HasIndex(e => e.CorrelationId, "IX_LocalRequest_CorrelationId");

		entityBuilder.HasIndex(e => e.IdRemoteSystem, "IXFK_LocalRequest_RemoteSystem");

		entityBuilder.Property(e => e.IdLocalRequest)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdRemoteSystem).HasColumnType("uuid");

		entityBuilder.Property(e => e.RemoteIp)
			.HasColumnType("varchar(63)")
			.HasMaxLength(63);

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.CorrelationId).HasColumnType("uuid");

		entityBuilder.Property(e => e.ExternalCorrelationId)
			.HasColumnType("varchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.SourceClientIdentifier)
			.IsRequired()
			.HasColumnType("varchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.Url)
			.IsRequired()
			.HasColumnType("varchar(2047)")
			.HasMaxLength(2047);

		entityBuilder.Property(e => e.Path)
			.HasColumnType("varchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.QueryString)
			.HasColumnType("varchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.Method)
			.HasColumnType("varchar(15)")
			.HasMaxLength(15);

		entityBuilder.Property(e => e.Headers).HasColumnType("jsonb");

		entityBuilder.Property(e => e.ContentType)
			.HasColumnType("varchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.Metadata).HasColumnType("jsonb");

		entityBuilder.Property(e => e.CustomCorrelationId)
			.HasColumnType("varchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.RuntimeUniqueKey).HasColumnType("uuid");

		entityBuilder.HasOne(d => d.RemoteSystem)
			.WithMany(p => p.LocalRequests)
			.HasForeignKey(d => d.IdRemoteSystem)
			.HasConstraintName("FK_LocalRequest_IdRemoteSystem");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Logs.Model.LocalRequest>(ConfigureEntity);

		return modelBuilder;
	}
}
