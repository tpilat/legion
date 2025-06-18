using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Auth.SqlServer;

public class RolePermissionConfiguration : IEntityTypeConfiguration<Auth.Model.RolePermission>
{
	public const string PrimaryKeyFormatter = "{{\"IdRolePermission\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Auth.Model.RolePermission> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Auth.Model.RolePermission> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdRolePermission);

		entityBuilder.ToTable("RolePermission", "auth");

		entityBuilder.HasIndex(e => e.IdPermission, "IXFK_RolePermission_Permission");

		entityBuilder.HasIndex(e => e.IdRole, "IXFK_RolePermission_Role");

		entityBuilder.HasIndex(e => new { e.IdRole, e.IdPermission, e.DeletedUtc }, "UQ_RolePermission_Role_Permission_Deleted")
				.IsUnique();

		entityBuilder.Property(e => e.IdRolePermission)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdRole).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdPermission).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.AuditCreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.AuditModifiedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.IdAuditCreatedBy).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdAuditModifiedBy).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.ConcurrencyToken)
			.HasColumnType("uniqueidentifier")
				.IsConcurrencyToken();

		entityBuilder.Property(e => e.DeletedUtc).HasColumnType("datetime2(7)");

		entityBuilder.HasOne(d => d.Permission)
			.WithMany(p => p.RolePermissions)
			.HasForeignKey(d => d.IdPermission)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_RolePermission_IdPermission");

		entityBuilder.HasOne(d => d.Role)
			.WithMany(p => p.RolePermissions)
			.HasForeignKey(d => d.IdRole)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_RolePermission_IdRole");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Auth.Model.RolePermission>(ConfigureEntity);

		return modelBuilder;
	}
}
