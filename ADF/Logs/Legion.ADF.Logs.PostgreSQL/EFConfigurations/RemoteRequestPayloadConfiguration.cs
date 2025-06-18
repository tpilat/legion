using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Logs.PostgreSQL;

public class RemoteRequestPayloadConfiguration : IEntityTypeConfiguration<Logs.Model.RemoteRequestPayload>
{
	public const string PrimaryKeyFormatter = "{{\"IdRemoteRequestPayload\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Logs.Model.RemoteRequestPayload> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Logs.Model.RemoteRequestPayload> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdRemoteRequestPayload);

		entityBuilder.ToTable("RemoteRequestPayload", "log");

		entityBuilder.HasIndex(e => e.IdRemoteRequest, "IXFK_RemoteRequestPayload_RemoteRequest");

		entityBuilder.Property(e => e.IdRemoteRequestPayload)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdRemoteRequest).HasColumnType("uuid");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.RequestContentType)
			.IsRequired()
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

		entityBuilder.HasOne(d => d.RemoteRequest)
			.WithMany(p => p.RemoteRequestPayloads)
			.HasForeignKey(d => d.IdRemoteRequest)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_RemoteRequestPayload_IdRemoteRequest");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Logs.Model.RemoteRequestPayload>(ConfigureEntity);

		return modelBuilder;
	}
}
