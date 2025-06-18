using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Auth.PostgreSQL;

public class PermissionConfiguration : IEntityTypeConfiguration<Auth.Model.Permission>
{
	public const string PrimaryKeyFormatter = "{{\"IdPermission\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Auth.Model.Permission> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Auth.Model.Permission> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdPermission);

		entityBuilder.ToTable("Permission", "auth");

		entityBuilder.HasIndex(e => e.Code, "UQ_Permission_Code")
				.IsUnique();

		entityBuilder.Property(e => e.IdPermission)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("varchar(256)")
			.HasMaxLength(256);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("varchar(1024)")
			.HasMaxLength(1024);

		entityBuilder.Property(e => e.ClaimValue)
			.HasColumnType("varchar(1024)")
			.HasMaxLength(1024);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Auth.Model.Permission>(ConfigureEntity);

		return modelBuilder;
	}
}
