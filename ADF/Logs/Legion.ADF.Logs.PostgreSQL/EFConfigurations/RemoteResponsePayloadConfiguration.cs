using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Logs.PostgreSQL;

public class RemoteResponsePayloadConfiguration : IEntityTypeConfiguration<Logs.Model.RemoteResponsePayload>
{
	public const string PrimaryKeyFormatter = "{{\"IdRemoteResponsePayload\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Logs.Model.RemoteResponsePayload> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Logs.Model.RemoteResponsePayload> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdRemoteResponsePayload);

		entityBuilder.ToTable("RemoteResponsePayload", "log");

		entityBuilder.HasIndex(e => e.IdRemoteResponse, "IXFK_RemoteResponsePayload_RemoteResponse");

		entityBuilder.Property(e => e.IdRemoteResponsePayload)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdRemoteResponse).HasColumnType("uuid");

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

		entityBuilder.HasOne(d => d.RemoteResponse)
			.WithMany(p => p.RemoteResponsePayloads)
			.HasForeignKey(d => d.IdRemoteResponse)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_RemoteResponsePayload_IdRemoteResponse");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Logs.Model.RemoteResponsePayload>(ConfigureEntity);

		return modelBuilder;
	}
}
