using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.Components.PostgreSQL;

public class AdapterRequestPayloadConfiguration : IEntityTypeConfiguration<Components.Model.AdapterRequestPayload>
{
	public const string PrimaryKeyFormatter = "{{\"IdAdapterRequestPayload\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Components.Model.AdapterRequestPayload> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Components.Model.AdapterRequestPayload> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdAdapterRequestPayload);

		entityBuilder.ToTable("AdapterRequestPayload", "comp");

		entityBuilder.HasIndex(e => e.IdAdapterRequest, "IXFK_AdapterRequestPayload_AdapterRequest");

		entityBuilder.Property(e => e.IdAdapterRequestPayload).ValueGeneratedNever();

		entityBuilder.Property(e => e.RequestContentType)
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

		entityBuilder.HasOne(d => d.AdapterRequest)
			.WithMany(p => p.AdapterRequestPayloads)
			.HasForeignKey(d => d.IdAdapterRequest)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_AdapterRequestPayload_IdAdapterRequest");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Components.Model.AdapterRequestPayload>(ConfigureEntity);

		return modelBuilder;
	}
}
