using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Auth.PostgreSQL;

public class UserPermissionConfiguration : IEntityTypeConfiguration<Auth.Model.UserPermission>
{
	public const string PrimaryKeyFormatter = "{{\"IdUserPermission\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Auth.Model.UserPermission> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Auth.Model.UserPermission> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdUserPermission);

		entityBuilder.ToTable("UserPermission", "auth");

		entityBuilder.HasIndex(e => new { e.DeletedUtc, e.IdPermission, e.IdUser, e.TenantIdentifier }, "UQ_UserPermission_User_Permission_Tenant_Deleted")
				.IsUnique();

		entityBuilder.HasIndex(e => e.IdPermission, "IXFK_UserPermission_Permission");

		entityBuilder.HasIndex(e => e.IdUser, "IXFK_UserPermission_User");

		entityBuilder.Property(e => e.IdUserPermission)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdUser).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdPermission).HasColumnType("uuid");

		entityBuilder.Property(e => e.TenantIdentifier).HasColumnType("uuid");

		entityBuilder.Property(e => e.AuditCreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.AuditModifiedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.IdAuditCreatedBy).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdAuditModifiedBy).HasColumnType("uuid");

		entityBuilder.Property(e => e.ConcurrencyToken)
			.HasColumnType("uuid")
				.IsConcurrencyToken();

		entityBuilder.Property(e => e.DeletedUtc).HasColumnType("timestamptz");

		entityBuilder.HasOne(d => d.Permission)
			.WithMany(p => p.UserPermissions)
			.HasForeignKey(d => d.IdPermission)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_UserPermission_IdPermission");

		entityBuilder.HasOne(d => d.User)
			.WithMany(p => p.UserPermissions)
			.HasForeignKey(d => d.IdUser)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_UserPermission_IdUser");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Auth.Model.UserPermission>(ConfigureEntity);

		return modelBuilder;
	}
}
