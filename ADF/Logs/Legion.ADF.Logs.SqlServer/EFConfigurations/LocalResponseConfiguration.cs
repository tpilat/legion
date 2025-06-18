using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Logs.SqlServer;

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
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdLocalRequest).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.CorrelationId).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.ExternalCorrelationId)
			.HasColumnType("nvarchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.StatusCode)
			.HasColumnType("nvarchar(63)")
			.HasMaxLength(63);

		entityBuilder.Property(e => e.Reason)
			.HasColumnType("nvarchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.Headers).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.ContentType)
			.HasColumnType("nvarchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.Error).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.ElapsedMilliseconds).HasColumnType("numeric(18, 0)");

		entityBuilder.Property(e => e.Metadata).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.CustomCorrelationId)
			.HasColumnType("nvarchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.RuntimeUniqueKey).HasColumnType("uniqueidentifier");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Logs.Model.LocalResponse>(ConfigureEntity);

		return modelBuilder;
	}
}
