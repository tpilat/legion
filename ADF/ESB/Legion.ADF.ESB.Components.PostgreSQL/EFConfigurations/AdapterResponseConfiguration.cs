using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.Components.PostgreSQL;

public class AdapterResponseConfiguration : IEntityTypeConfiguration<Components.Model.AdapterResponse>
{
	public const string PrimaryKeyFormatter = "{{\"IdAdapterResponse\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Components.Model.AdapterResponse> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Components.Model.AdapterResponse> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdAdapterResponse);

		entityBuilder.ToTable("AdapterResponse", "comp");

		entityBuilder.HasIndex(e => e.IdAdapter, "IXFK_AdapterResponse_IdAdapter");

		entityBuilder.HasIndex(e => e.IdAdapterRequest, "IXFK_AdapterResponse_IdAdapterRequest");

		entityBuilder.Property(e => e.IdAdapterResponse).ValueGeneratedNever();

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamp(6)");

		entityBuilder.Property(e => e.Properties).HasColumnType("jsonb");

		entityBuilder.Property(e => e.Headers).HasColumnType("jsonb");

		entityBuilder.Property(e => e.ContentType).HasMaxLength(255);

		entityBuilder.HasOne(d => d.Adapter)
			.WithMany(p => p.AdapterResponses)
			.HasForeignKey(d => d.IdAdapter)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_AdapterResponse_IdAdapter");

		entityBuilder.HasOne(d => d.AdapterRequest)
			.WithMany(p => p.AdapterResponses)
			.HasForeignKey(d => d.IdAdapterRequest)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_AdapterResponse_IdAdapterRequest");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Components.Model.AdapterResponse>(ConfigureEntity);

		return modelBuilder;
	}
}
