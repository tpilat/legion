using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.Components.PostgreSQL;

public class AdapterResponsePayloadConfiguration : IEntityTypeConfiguration<Components.Model.AdapterResponsePayload>
{
	public const string PrimaryKeyFormatter = "{{\"IdAdapterResponsePayload\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Components.Model.AdapterResponsePayload> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Components.Model.AdapterResponsePayload> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdAdapterResponsePayload);

		entityBuilder.ToTable("AdapterResponsePayload", "comp");

		entityBuilder.HasIndex(e => e.IdAdapterResponse, "IXFK_AdapterResponsePayload_AdapterResponse");

		entityBuilder.Property(e => e.IdAdapterResponsePayload).ValueGeneratedNever();

		entityBuilder.Property(e => e.ResponseContentType)
			.IsRequired()
			.HasMaxLength(63);

		entityBuilder.Property(e => e.JsonContent).HasColumnType("jsonb");

		entityBuilder.Property(e => e.ContentHeaders).HasColumnType("jsonb");

		entityBuilder.Property(e => e.Name).HasMaxLength(511);

		entityBuilder.Property(e => e.RelativePath).HasMaxLength(1023);

		entityBuilder.Property(e => e.Metadata).HasColumnType("jsonb");

		entityBuilder.Property(e => e.ContentEncoding).HasMaxLength(63);

		entityBuilder.Property(e => e.MediaType).HasMaxLength(255);

		entityBuilder.Property(e => e.MultipartFormDataContentName).HasMaxLength(511);

		entityBuilder.Property(e => e.MultipartFormDataFileName).HasMaxLength(511);

		entityBuilder.Property(e => e.JsonInputCSharpType).HasMaxLength(1023);

		entityBuilder.HasOne(d => d.AdapterResponse)
			.WithMany(p => p.AdapterResponsePayloads)
			.HasForeignKey(d => d.IdAdapterResponse)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_AdapterResponsePayload_IdAdapterResponse");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Components.Model.AdapterResponsePayload>(ConfigureEntity);

		return modelBuilder;
	}
}
