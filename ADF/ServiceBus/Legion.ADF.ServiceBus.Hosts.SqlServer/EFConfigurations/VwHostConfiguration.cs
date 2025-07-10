using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.Hosts.SqlServer;

public class VwHostConfiguration : IEntityTypeConfiguration<Hosts.Model.VwHost>
{
	public void Configure(EntityTypeBuilder<Hosts.Model.VwHost> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Hosts.Model.VwHost> entityBuilder)
	{
		entityBuilder.ToView("VwHost", "hosts")
			.HasNoKey();

		entityBuilder.Property(e => e.IdHost).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("varchar(255)");

		entityBuilder.Property(e => e.Description)
			.IsRequired()
			.HasColumnType("varchar(511)");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.IsEnabled).HasColumnType("bit");

		entityBuilder.Property(e => e.StartedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.LastActivityUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.StoppedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.Configuration)
			.IsRequired()
			.HasColumnType("nvarchar(max)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Hosts.Model.VwHost>(ConfigureEntity);
}
