using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.PostgreSQL;

public class VwHostConfiguration : IEntityTypeConfiguration<ServiceBus.Model.VwHost>
{
	public void Configure(EntityTypeBuilder<ServiceBus.Model.VwHost> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<ServiceBus.Model.VwHost> entityBuilder)
	{
		entityBuilder.ToView("VwHost", "hosts")
			.HasNoKey();

		entityBuilder.Property(e => e.IdHost).HasColumnType("uuid");

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("varchar(255)");

		entityBuilder.Property(e => e.Description)
			.IsRequired()
			.HasColumnType("varchar(511)");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.Configuration)
			.IsRequired()
			.HasColumnType("jsonb");

		entityBuilder.Property(e => e.RowVersion).HasColumnType("uuid");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<ServiceBus.Model.VwHost>(ConfigureEntity);
}
