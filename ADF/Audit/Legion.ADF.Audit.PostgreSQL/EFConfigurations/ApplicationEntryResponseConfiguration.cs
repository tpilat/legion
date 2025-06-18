using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Audit.PostgreSQL;

public class ApplicationEntryResponseConfiguration : IEntityTypeConfiguration<Audit.Model.ApplicationEntryResponse>
{
	public const string PrimaryKeyFormatter = "{{\"IdApplicationEntryResponse\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Audit.Model.ApplicationEntryResponse> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Audit.Model.ApplicationEntryResponse> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdApplicationEntryResponse);

		entityBuilder.ToTable("ApplicationEntryResponse", "aud");

		entityBuilder.HasIndex(e => e.IdApplicationEntry, "IXFK_ApplicationEntryResponse_ApplicationEntry");

		entityBuilder.Property(e => e.IdApplicationEntryResponse)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdApplicationEntry).HasColumnType("uuid");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.StatusCode)
			.HasColumnType("varchar(63)")
			.HasMaxLength(63);

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
			.WithMany(p => p.ApplicationEntryResponses)
			.HasForeignKey(d => d.IdApplicationEntry)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_ApplicationEntryResponse_IdApplicationEntry");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Audit.Model.ApplicationEntryResponse>(ConfigureEntity);

		return modelBuilder;
	}
}
