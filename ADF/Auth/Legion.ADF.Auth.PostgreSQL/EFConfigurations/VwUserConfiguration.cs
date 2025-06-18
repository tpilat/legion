using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Auth.PostgreSQL;

public class VwUserConfiguration : IEntityTypeConfiguration<Auth.Model.VwUser>
{
	public void Configure(EntityTypeBuilder<Auth.Model.VwUser> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Auth.Model.VwUser> entityBuilder)
	{
		entityBuilder.ToView("VwUser", "auth")
			.HasNoKey();

		entityBuilder.Property(e => e.IdUser).HasColumnType("uuid");

		entityBuilder.Property(e => e.Login).HasColumnType("varchar(256)");

		entityBuilder.Property(e => e.NormalizedLogin).HasColumnType("varchar(256)");

		entityBuilder.Property(e => e.TenantIdentifier).HasColumnType("uuid");

		entityBuilder.Property(e => e.Email).HasColumnType("varchar(256)");

		entityBuilder.Property(e => e.NormalizedEmail).HasColumnType("varchar(256)");

		entityBuilder.Property(e => e.Data).HasColumnType("jsonb");

		entityBuilder.Property(e => e.PhoneNumber).HasColumnType("varchar(256)");

		entityBuilder.Property(e => e.LockoutEndUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.ConfirmationUrlValidToUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.AuditCreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.AuditModifiedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.IdAuditCreatedBy).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdAuditModifiedBy).HasColumnType("uuid");

		entityBuilder.Property(e => e.ConcurrencyToken).HasColumnType("uuid");

		entityBuilder.Property(e => e.DeletedUtc).HasColumnType("timestamptz");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Auth.Model.VwUser>(ConfigureEntity);
}
