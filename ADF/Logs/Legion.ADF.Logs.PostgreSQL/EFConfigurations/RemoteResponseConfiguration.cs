using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Logs.PostgreSQL;

public class RemoteResponseConfiguration : IEntityTypeConfiguration<Logs.Model.RemoteResponse>
{
	public const string PrimaryKeyFormatter = "{{\"IdRemoteResponse\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Logs.Model.RemoteResponse> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Logs.Model.RemoteResponse> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdRemoteResponse);

		entityBuilder.ToTable("RemoteResponse", "log");

		entityBuilder.HasIndex(e => e.CorrelationId, "IX_RemoteResponse_CorrelationId");

		entityBuilder.HasIndex(e => e.IdRemoteRequest, "IXFK_RemoteResponse_RemoteRequest");

		entityBuilder.Property(e => e.IdRemoteResponse)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdRemoteRequest).HasColumnType("uuid");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.CorrelationId).HasColumnType("uuid");

		entityBuilder.Property(e => e.ExternalCorrelationId)
			.HasColumnType("varchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.StatusCode)
			.HasColumnType("varchar(63)")
			.HasMaxLength(63);

		entityBuilder.Property(e => e.Reason)
			.HasColumnType("varchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.Headers).HasColumnType("jsonb");

		entityBuilder.Property(e => e.ContentType)
			.HasColumnType("varchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.Metadata).HasColumnType("jsonb");

		entityBuilder.Property(e => e.CustomCorrelationId)
			.HasColumnType("varchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.RuntimeUniqueKey).HasColumnType("uuid");

		entityBuilder.HasOne(d => d.RemoteRequest)
			.WithMany(p => p.RemoteResponses)
			.HasForeignKey(d => d.IdRemoteRequest)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_RemoteResponse_IdRemoteRequest");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Logs.Model.RemoteResponse>(ConfigureEntity);

		return modelBuilder;
	}
}
