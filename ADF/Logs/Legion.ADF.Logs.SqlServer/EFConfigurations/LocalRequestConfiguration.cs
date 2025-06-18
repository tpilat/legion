using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Logs.SqlServer;

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
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdRemoteSystem).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.RemoteIp)
			.HasColumnType("nvarchar(63)")
			.HasMaxLength(63);

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

		entityBuilder.Property(e => e.Path)
			.HasColumnType("nvarchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.QueryString)
			.HasColumnType("nvarchar(1023)")
			.HasMaxLength(1023);

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
		modelBuilder.Entity<Logs.Model.LocalRequest>(ConfigureEntity);

		return modelBuilder;
	}
}
