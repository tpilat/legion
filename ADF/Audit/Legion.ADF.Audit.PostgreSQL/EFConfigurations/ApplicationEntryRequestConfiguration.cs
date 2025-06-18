using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Audit.PostgreSQL;

public class ApplicationEntryRequestConfiguration : IEntityTypeConfiguration<Audit.Model.ApplicationEntryRequest>
{
	public const string PrimaryKeyFormatter = "{{\"IdApplicationEntryRequest\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Audit.Model.ApplicationEntryRequest> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Audit.Model.ApplicationEntryRequest> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdApplicationEntryRequest);

		entityBuilder.ToTable("ApplicationEntryRequest", "aud");

		entityBuilder.HasIndex(e => e.IdApplicationEntry, "IXFK_ApplicationEntryRequest_ApplicationEntry");

		entityBuilder.Property(e => e.IdApplicationEntryRequest)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdApplicationEntry).HasColumnType("uuid");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.Metadata).HasColumnType("jsonb");

		entityBuilder.Property(e => e.MimeType)
			.IsRequired()
			.HasColumnType("varchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.ContentEncoding)
			.HasColumnType("varchar(63)")
			.HasMaxLength(63);

		entityBuilder.Property(e => e.ByteArrayContent).HasColumnType("bytea");

		entityBuilder.Property(e => e.JsonContent).HasColumnType("jsonb");

		entityBuilder.Property(e => e.Name)
			.HasColumnType("varchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.RelativePath)
			.HasColumnType("varchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.HasOne(d => d.ApplicationEntry)
			.WithMany(p => p.ApplicationEntryRequests)
			.HasForeignKey(d => d.IdApplicationEntry)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_ApplicationEntryRequest_IdApplicationEntry");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Audit.Model.ApplicationEntryRequest>(ConfigureEntity);

		return modelBuilder;
	}
}
