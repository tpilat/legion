using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Logs.PostgreSQL;

public class LocalResponseConfiguration : IEntityTypeConfiguration<Logs.Model.LocalResponse>
{
	public const string PrimaryKeyFormatter = "{{\"IdLocalResponse\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Logs.Model.LocalResponse> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Logs.Model.LocalResponse> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdLocalResponse);

		entityBuilder.ToTable("LocalResponse", "log");

		entityBuilder.HasIndex(e => e.CorrelationId, "IX_LocalResponse_CorrelationId");

		entityBuilder.HasIndex(e => e.IdLocalRequest, "IXFK_LocalResponse_LocalRequest");

		entityBuilder.Property(e => e.IdLocalResponse)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdLocalRequest).HasColumnType("uuid");

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

		entityBuilder.HasOne(d => d.LocalRequest)
			.WithMany(p => p.LocalResponses)
			.HasForeignKey(d => d.IdLocalRequest)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_LocalResponse_IdLocalRequest");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Logs.Model.LocalResponse>(ConfigureEntity);

		return modelBuilder;
	}
}
