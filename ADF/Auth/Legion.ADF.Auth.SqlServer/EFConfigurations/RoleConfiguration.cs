using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Auth.SqlServer;

public class RoleConfiguration : IEntityTypeConfiguration<Auth.Model.Role>
{
	public const string PrimaryKeyFormatter = "{{\"IdRole\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Auth.Model.Role> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Auth.Model.Role> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdRole);

		entityBuilder.ToTable("Role", "auth");

		entityBuilder.HasIndex(e => new { e.Name, e.DeletedUtc }, "UQ_Role_Name_Deleted")
				.IsUnique();

		entityBuilder.Property(e => e.IdRole)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("nvarchar(256)")
			.HasMaxLength(256);

		entityBuilder.Property(e => e.NormalizedName)
			.IsRequired()
			.HasColumnType("nvarchar(256)")
			.HasMaxLength(256);

		entityBuilder.Property(e => e.ADGroupDistinguishedName).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.Data).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.Description).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.HasConstantPermissions).HasColumnType("bit");

		entityBuilder.Property(e => e.HasConstantUsers).HasColumnType("bit");

		entityBuilder.Property(e => e.IsSystemRole).HasColumnType("bit");

		entityBuilder.Property(e => e.AuditCreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.AuditModifiedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.IdAuditCreatedBy).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdAuditModifiedBy).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.ConcurrencyToken)
			.HasColumnType("uniqueidentifier")
				.IsConcurrencyToken();

		entityBuilder.Property(e => e.DeletedUtc).HasColumnType("datetime2(7)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Auth.Model.Role>(ConfigureEntity);

		return modelBuilder;
	}
}
