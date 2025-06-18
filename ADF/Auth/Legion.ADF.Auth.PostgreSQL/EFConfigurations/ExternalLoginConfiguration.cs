using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Auth.PostgreSQL;

public class ExternalLoginConfiguration : IEntityTypeConfiguration<Auth.Model.ExternalLogin>
{
	public const string PrimaryKeyFormatter = "{{\"IdExternalLogin\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Auth.Model.ExternalLogin> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Auth.Model.ExternalLogin> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdExternalLogin);

		entityBuilder.ToTable("ExternalLogin", "auth");

		entityBuilder.HasIndex(e => new { e.ExternalUserIdentifier, e.IdLoginProvider, e.IdUser }, "UQ_ExternalLogin_IdProvider_IdUser_Identifier")
				.IsUnique();

		entityBuilder.HasIndex(e => e.IdLoginProvider, "IXFK_ExternalLogin_LoginProvider");

		entityBuilder.HasIndex(e => e.IdUser, "IXFK_ExternalLogin_User");

		entityBuilder.Property(e => e.IdExternalLogin)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdLoginProvider).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdUser).HasColumnType("uuid");

		entityBuilder.Property(e => e.ExternalUserIdentifier).IsRequired();

		entityBuilder.Property(e => e.Data).HasColumnType("jsonb");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.ValidToUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.LastAccessUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.RemoteIP)
			.HasColumnType("varchar(64)")
			.HasMaxLength(64);

		entityBuilder.HasOne(d => d.LoginProvider)
			.WithMany(p => p.ExternalLogins)
			.HasForeignKey(d => d.IdLoginProvider)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_ExternalLogin_IdLoginProvider");

		entityBuilder.HasOne(d => d.User)
			.WithMany(p => p.ExternalLogins)
			.HasForeignKey(d => d.IdUser)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_ExternalLogin_IdUser");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Auth.Model.ExternalLogin>(ConfigureEntity);

		return modelBuilder;
	}
}
