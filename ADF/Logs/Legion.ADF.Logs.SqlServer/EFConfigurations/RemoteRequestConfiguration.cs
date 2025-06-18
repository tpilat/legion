using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Logs.SqlServer;

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
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdRemoteSystem).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.CorrelationId).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.ExternalCorrelationId)
			.HasColumnType("nvarchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.SourceClientIdentifier)
			.IsRequired()
			.HasColumnType("nvarchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.Url)
			.IsRequired()
			.HasColumnType("nvarchar(2047)")
			.HasMaxLength(2047);

		entityBuilder.Property(e => e.Method)
			.HasColumnType("nvarchar(15)")
			.HasMaxLength(15);

		entityBuilder.Property(e => e.Headers).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.ContentType)
			.HasColumnType("nvarchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.Metadata).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.CustomCorrelationId)
			.HasColumnType("nvarchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.RuntimeUniqueKey).HasColumnType("uniqueidentifier");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Logs.Model.RemoteRequest>(ConfigureEntity);

		return modelBuilder;
	}
}
