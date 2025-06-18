using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Logs.SqlServer;

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
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdRemoteRequest).HasColumnType("uniqueidentifier");

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
		modelBuilder.Entity<Logs.Model.RemoteResponse>(ConfigureEntity);

		return modelBuilder;
	}
}
