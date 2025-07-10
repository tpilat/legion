using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.Hosts.PostgreSQL;

public class VwHostConfiguration : IEntityTypeConfiguration<Hosts.Model.VwHost>
{
	public void Configure(EntityTypeBuilder<Hosts.Model.VwHost> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Hosts.Model.VwHost> entityBuilder)
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

		entityBuilder.Property(e => e.StartedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.LastActivityUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.StoppedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.Configuration)
			.IsRequired()
			.HasColumnType("jsonb");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Hosts.Model.VwHost>(ConfigureEntity);
}
