using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Logs.PostgreSQL;

public class RemoteSystemConfiguration : IEntityTypeConfiguration<Logs.Model.RemoteSystem>
{
	public const string PrimaryKeyFormatter = "{{\"IdRemoteSystem\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Logs.Model.RemoteSystem> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Logs.Model.RemoteSystem> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdRemoteSystem);

		entityBuilder.ToTable("RemoteSystem", "log");

		entityBuilder.HasIndex(e => e.Code, "UQ_RemoteSystem_Code")
				.IsUnique();

		entityBuilder.Property(e => e.IdRemoteSystem)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("varchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("varchar(127)")
			.HasMaxLength(127);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Logs.Model.RemoteSystem>(ConfigureEntity);

		return modelBuilder;
	}
}
