using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Auth.SqlServer;

public class UserRoleConfiguration : IEntityTypeConfiguration<Auth.Model.UserRole>
{
	public const string PrimaryKeyFormatter = "{{\"IdUserRole\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Auth.Model.UserRole> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Auth.Model.UserRole> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdUserRole);

		entityBuilder.ToTable("UserRole", "auth");

		entityBuilder.HasIndex(e => e.IdRole, "IXFK_UserRole_Role");

		entityBuilder.HasIndex(e => e.IdUser, "IXFK_UserRole_User");

		entityBuilder.HasIndex(e => new { e.IdUser, e.IdRole, e.TenantIdentifier, e.DeletedUtc }, "UQ_UserRole_User_Role_Tenant_Deleted")
				.IsUnique();

		entityBuilder.Property(e => e.IdUserRole)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdUser).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdRole).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.TenantIdentifier).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.AuditCreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.AuditModifiedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.IdAuditCreatedBy).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdAuditModifiedBy).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.ConcurrencyToken)
			.HasColumnType("uniqueidentifier")
				.IsConcurrencyToken();

		entityBuilder.Property(e => e.DeletedUtc).HasColumnType("datetime2(7)");

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
