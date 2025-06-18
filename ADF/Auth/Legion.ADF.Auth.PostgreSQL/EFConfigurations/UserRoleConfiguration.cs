using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Auth.PostgreSQL;

public class UserRoleConfiguration : IEntityTypeConfiguration<Auth.Model.UserRole>
{
	public const string PrimaryKeyFormatter = "{{\"IdUserRole\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Auth.Model.UserRole> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Auth.Model.UserRole> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdUserRole);

		entityBuilder.ToTable("UserRole", "auth");

		entityBuilder.HasIndex(e => new { e.DeletedUtc, e.IdRole, e.IdUser, e.TenantIdentifier }, "UQ_UserRole_User_Role_Tenant_Deleted")
				.IsUnique();

		entityBuilder.HasIndex(e => e.IdRole, "IXFK_UserRole_Role");

		entityBuilder.HasIndex(e => e.IdUser, "IXFK_UserRole_User");

		entityBuilder.Property(e => e.IdUserRole)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdUser).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdRole).HasColumnType("uuid");

		entityBuilder.Property(e => e.TenantIdentifier).HasColumnType("uuid");

		entityBuilder.Property(e => e.AuditCreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.AuditModifiedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.IdAuditCreatedBy).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdAuditModifiedBy).HasColumnType("uuid");

		entityBuilder.Property(e => e.ConcurrencyToken)
			.HasColumnType("uuid")
				.IsConcurrencyToken();

		entityBuilder.Property(e => e.DeletedUtc).HasColumnType("timestamptz");

		entityBuilder.HasOne(d => d.Role)
			.WithMany(p => p.UserRoles)
			.HasForeignKey(d => d.IdRole)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_UserRole_IdRole");

		entityBuilder.HasOne(d => d.User)
			.WithMany(p => p.UserRoles)
			.HasForeignKey(d => d.IdUser)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_UserRole_IdUser");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Auth.Model.UserRole>(ConfigureEntity);

		return modelBuilder;
	}
}
