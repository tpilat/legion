using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.SqlServer;

public class HostActivityConfiguration : IEntityTypeConfiguration<ServiceBus.Model.HostActivity>
{
	public const string PrimaryKeyFormatter = "{{\"IdHostActivity\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<ServiceBus.Model.HostActivity> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<ServiceBus.Model.HostActivity> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdHostActivity);

		entityBuilder.ToTable("HostActivity", "hosts");

		entityBuilder.HasIndex(e => e.IdHost, "IXFK_HostActivity_Host");

		entityBuilder.HasIndex(e => e.IdHost, "UQ_HostActivity_IdHost")
				.IsUnique();

		entityBuilder.Property(e => e.IdHostActivity)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdHost).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.StartedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.LastActivityUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.StoppedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.IsDistributedManagerAvailable).HasColumnType("bit");

		entityBuilder.Property(e => e.RowVersion)
			.HasColumnType("uniqueidentifier")
				.IsConcurrencyToken();

		entityBuilder.HasOne(d => d.Host)
			.WithOne(p => p.HostActivity)
			.HasForeignKey<ServiceBus.Model.HostActivity>(d => d.IdHost)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_HostActivity_IdHost");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<ServiceBus.Model.HostActivity>(ConfigureEntity);

		return modelBuilder;
	}
}
