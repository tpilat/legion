using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Logs.SqlServer;

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
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdRemoteResponse).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.ResponseContentType)
			.IsRequired()
			.HasColumnType("nvarchar(63)")
			.HasMaxLength(63);

		entityBuilder.Property(e => e.ByteArrayContent).HasColumnType("varbinary(max)");

		entityBuilder.Property(e => e.JsonContent).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.StringContent).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.ContentHeaders).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.FileName)
			.HasColumnType("nvarchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.RelativePath)
			.HasColumnType("nvarchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.Metadata).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.IsCompressed).HasColumnType("bit");

		entityBuilder.Property(e => e.EncryptionKey).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.ContentEncoding)
			.HasColumnType("nvarchar(63)")
			.HasMaxLength(63);

		entityBuilder.Property(e => e.MediaType)
			.HasColumnType("nvarchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.MultipartFormDataContentName)
			.HasColumnType("nvarchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.MultipartFormDataFileName)
			.HasColumnType("nvarchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.JsonInputCSharpType)
			.HasColumnType("nvarchar(1023)")
			.HasMaxLength(1023);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Logs.Model.RemoteResponsePayload>(ConfigureEntity);

		return modelBuilder;
	}
}
