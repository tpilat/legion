using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Auth.SqlServer;

public class VwUserConfiguration : IEntityTypeConfiguration<Auth.Model.VwUser>
{
	public void Configure(EntityTypeBuilder<Auth.Model.VwUser> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Auth.Model.VwUser> entityBuilder)
	{
		entityBuilder.ToView("VwUser", "auth")
			.HasNoKey();

		entityBuilder.Property(e => e.IdUser).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.Login).HasColumnType("nvarchar(256)");

		entityBuilder.Property(e => e.NormalizedLogin).HasColumnType("nvarchar(256)");

		entityBuilder.Property(e => e.TenantIdentifier).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.Email).HasColumnType("nvarchar(256)");

		entityBuilder.Property(e => e.NormalizedEmail).HasColumnType("nvarchar(256)");

		entityBuilder.Property(e => e.EmailConfirmed).HasColumnType("bit");

		entityBuilder.Property(e => e.PasswordHash).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.SecurityStamp).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.ADDistinguishedName).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.Data).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.PhoneNumber).HasColumnType("nvarchar(256)");

		entityBuilder.Property(e => e.PhoneNumberConfirmed).HasColumnType("bit");

		entityBuilder.Property(e => e.MultiFactorEnabled).HasColumnType("bit");

		entityBuilder.Property(e => e.LockoutEndUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.LockoutEnabled).HasColumnType("bit");

		entityBuilder.Property(e => e.IsSystemUser).HasColumnType("bit");

		entityBuilder.Property(e => e.ConfirmationUrlSlug).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.ConfirmationUrlValidToUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.AuditCreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.AuditModifiedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.IdAuditCreatedBy).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdAuditModifiedBy).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.ConcurrencyToken).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.DeletedUtc).HasColumnType("datetime2(7)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Auth.Model.VwUser>(ConfigureEntity);
}
