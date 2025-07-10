using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.Hosts.SqlServer;

public class HostConfiguration : IEntityTypeConfiguration<Hosts.Model.Host>
{
	public const string PrimaryKeyFormatter = "{{\"IdHost\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Hosts.Model.Host> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Hosts.Model.Host> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdHost);

		entityBuilder.ToTable("Host", "hosts");

		entityBuilder.HasIndex(e => e.Name, "UQ_Host_Name")
				.IsUnique();

		entityBuilder.Property(e => e.IdHost)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("varchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.Description)
			.IsRequired()
			.HasColumnType("varchar(511)")
			.HasMaxLength(511);

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
	{
		modelBuilder.Entity<Hosts.Model.Host>(ConfigureEntity);

		return modelBuilder;
	}
}
