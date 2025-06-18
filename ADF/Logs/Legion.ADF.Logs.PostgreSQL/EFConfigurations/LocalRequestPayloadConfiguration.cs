using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Logs.PostgreSQL;

public class LocalRequestPayloadConfiguration : IEntityTypeConfiguration<Logs.Model.LocalRequestPayload>
{
	public const string PrimaryKeyFormatter = "{{\"IdLocalRequestPayload\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Logs.Model.LocalRequestPayload> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Logs.Model.LocalRequestPayload> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdLocalRequestPayload);

		entityBuilder.ToTable("LocalRequestPayload", "log");

		entityBuilder.HasIndex(e => e.IdLocalRequest, "IXFK_LocalRequestPayload_LocalRequest");

		entityBuilder.Property(e => e.IdLocalRequestPayload)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdLocalRequest).HasColumnType("uuid");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.RequestContentType)
			.HasColumnType("varchar(127)")
			.HasMaxLength(127);

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

		entityBuilder.HasOne(d => d.LocalRequest)
			.WithMany(p => p.LocalRequestPayloads)
			.HasForeignKey(d => d.IdLocalRequest)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_LocalRequestPayload_IdLocalRequest");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Logs.Model.LocalRequestPayload>(ConfigureEntity);

		return modelBuilder;
	}
}
