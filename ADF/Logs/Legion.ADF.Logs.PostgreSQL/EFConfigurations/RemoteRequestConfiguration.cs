using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Logs.PostgreSQL;

public class RemoteRequestConfiguration : IEntityTypeConfiguration<Logs.Model.RemoteRequest>
{
	public const string PrimaryKeyFormatter = "{{\"IdRemoteRequest\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Logs.Model.RemoteRequest> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Logs.Model.RemoteRequest> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdRemoteRequest);

		entityBuilder.ToTable("RemoteRequest", "log");

		entityBuilder.HasIndex(e => e.CorrelationId, "IX_RemoteRequest_CorrelationId");

		entityBuilder.HasIndex(e => e.IdRemoteSystem, "IXFK_RemoteRequest_RemoteSystem");

		entityBuilder.Property(e => e.IdRemoteRequest)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdRemoteSystem).HasColumnType("uuid");

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
			.WithMany(p => p.RemoteRequests)
			.HasForeignKey(d => d.IdRemoteSystem)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_RemoteRequest_IdRemoteSystem");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Logs.Model.RemoteRequest>(ConfigureEntity);

		return modelBuilder;
	}
}
