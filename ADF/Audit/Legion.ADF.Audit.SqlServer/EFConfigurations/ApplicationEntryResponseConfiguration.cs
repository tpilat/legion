using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Audit.SqlServer;

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
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdApplicationEntry).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.ElapsedMilliseconds).HasColumnType("numeric(18, 0)");

		entityBuilder.Property(e => e.StatusCode)
			.HasColumnType("varchar(63)")
			.HasMaxLength(63);

		entityBuilder.Property(e => e.Metadata).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.Error).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.MimeType)
			.HasColumnType("nvarchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.ContentEncoding)
			.IsRequired()
			.HasColumnType("varchar(63)")
			.HasMaxLength(63);

		entityBuilder.Property(e => e.ByteArrayContent).HasColumnType("varbinary(max)");

		entityBuilder.Property(e => e.JsonContent).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.StringContent).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.Name)
			.HasColumnType("varchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.RelativePath)
			.HasColumnType("varchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.IsCompressed).HasColumnType("bit");

		entityBuilder.Property(e => e.EncryptionKey).HasColumnType("nvarchar(max)");

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
