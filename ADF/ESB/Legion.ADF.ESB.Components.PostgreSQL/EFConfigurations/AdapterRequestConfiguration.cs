using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.Components.PostgreSQL;

public class AdapterRequestConfiguration : IEntityTypeConfiguration<Components.Model.AdapterRequest>
{
	public const string PrimaryKeyFormatter = "{{\"IdAdapterRequest\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Components.Model.AdapterRequest> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Components.Model.AdapterRequest> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdAdapterRequest);

		entityBuilder.ToTable("AdapterRequest", "comp");

		entityBuilder.HasIndex(e => e.IdAdapter, "IXFK_AdapterRequest_IdAdapter");

		entityBuilder.Property(e => e.IdAdapterRequest).ValueGeneratedNever();

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamp(6)");

		entityBuilder.Property(e => e.Properties).HasColumnType("jsonb");

		entityBuilder.Property(e => e.Identifier)
			.IsRequired()
			.HasMaxLength(127);

		entityBuilder.Property(e => e.Url)
			.IsRequired()
			.HasMaxLength(2047);

		entityBuilder.Property(e => e.Method).HasMaxLength(15);

		entityBuilder.Property(e => e.Headers).HasColumnType("jsonb");

		entityBuilder.Property(e => e.ContentType).HasMaxLength(255);

		entityBuilder.HasOne(d => d.Adapter)
			.WithMany(p => p.AdapterRequests)
			.HasForeignKey(d => d.IdAdapter)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_AdapterRequest_IdAdapter");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Components.Model.AdapterRequest>(ConfigureEntity);

		return modelBuilder;
	}
}
