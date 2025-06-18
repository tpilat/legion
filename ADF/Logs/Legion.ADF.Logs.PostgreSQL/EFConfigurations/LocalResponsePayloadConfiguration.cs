using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Logs.PostgreSQL;

public class LocalResponsePayloadConfiguration : IEntityTypeConfiguration<Logs.Model.LocalResponsePayload>
{
	public const string PrimaryKeyFormatter = "{{\"IdLocalResponsePayload\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Logs.Model.LocalResponsePayload> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Logs.Model.LocalResponsePayload> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdLocalResponsePayload);

		entityBuilder.ToTable("LocalResponsePayload", "log");

		entityBuilder.HasIndex(e => e.IdLocalResponse, "IXFK_LocalResponsePayload_LocalResponse");

		entityBuilder.Property(e => e.IdLocalResponsePayload)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdLocalResponse).HasColumnType("uuid");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.ResponseContentType)
			.IsRequired()
			.HasColumnType("varchar(63)")
			.HasMaxLength(63);

		entityBuilder.Property(e => e.ByteArrayContent).HasColumnType("bytea");

		entityBuilder.Property(e => e.JsonContent).HasColumnType("jsonb");

		entityBuilder.Property(e => e.ContentHeaders).HasColumnType("jsonb");

		entityBuilder.Property(e => e.FileName)
			.HasColumnType("varchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.RelativePath)
			.HasColumnType("varchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.Metadata).HasColumnType("jsonb");

		entityBuilder.Property(e => e.ContentEncoding)
			.HasColumnType("varchar(63)")
			.HasMaxLength(63);

		entityBuilder.Property(e => e.MediaType)
			.HasColumnType("varchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.MultipartFormDataContentName)
			.HasColumnType("varchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.MultipartFormDataFileName)
			.HasColumnType("varchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.JsonInputCSharpType)
			.HasColumnType("varchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.HasOne(d => d.LocalResponse)
			.WithMany(p => p.LocalResponsePayloads)
			.HasForeignKey(d => d.IdLocalResponse)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_LocalResponsePayload_IdLocalResponse");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Logs.Model.LocalResponsePayload>(ConfigureEntity);

		return modelBuilder;
	}
}
