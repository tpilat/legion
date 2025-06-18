using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Auth.PostgreSQL;

public class UserConfiguration : IEntityTypeConfiguration<Auth.Model.User>
{
	public const string PrimaryKeyFormatter = "{{\"IdUser\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Auth.Model.User> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Auth.Model.User> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdUser);

		entityBuilder.ToTable("User", "auth");

		entityBuilder.HasIndex(e => new { e.DeletedUtc, e.Login }, "UQ_User_Login_Deleted")
				.IsUnique();

		entityBuilder.Property(e => e.IdUser)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Login)
			.HasColumnType("varchar(256)")
			.HasMaxLength(256);

		entityBuilder.Property(e => e.NormalizedLogin)
			.HasColumnType("varchar(256)")
			.HasMaxLength(256);

		entityBuilder.Property(e => e.TenantIdentifier).HasColumnType("uuid");

		entityBuilder.Property(e => e.Email)
			.HasColumnType("varchar(256)")
			.HasMaxLength(256);

		entityBuilder.Property(e => e.NormalizedEmail)
			.HasColumnType("varchar(256)")
			.HasMaxLength(256);

		entityBuilder.Property(e => e.Data).HasColumnType("jsonb");

		entityBuilder.Property(e => e.PhoneNumber)
			.HasColumnType("varchar(256)")
			.HasMaxLength(256);

		entityBuilder.Property(e => e.LockoutEndUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.ConfirmationUrlValidToUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.AuditCreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.AuditModifiedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.IdAuditCreatedBy).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdAuditModifiedBy).HasColumnType("uuid");

		entityBuilder.Property(e => e.ConcurrencyToken)
			.HasColumnType("uuid")
				.IsConcurrencyToken();

		entityBuilder.Property(e => e.DeletedUtc).HasColumnType("timestamptz");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Auth.Model.User>(ConfigureEntity);

		return modelBuilder;
	}
}
