using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Auth.SqlServer;

public class UserTokenConfiguration : IEntityTypeConfiguration<Auth.Model.UserToken>
{
	public const string PrimaryKeyFormatter = "{{\"IdUserToken\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Auth.Model.UserToken> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Auth.Model.UserToken> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdUserToken);

		entityBuilder.ToTable("UserToken", "auth");

		entityBuilder.HasIndex(e => e.IdLoginProvider, "IXFK_UserToken_LoginProvider");

		entityBuilder.HasIndex(e => new { e.IdLoginProvider, e.IdUser, e.Value }, "UQ_UserToken_IdProvider_IdUser_Value")
				.IsUnique();

		entityBuilder.HasIndex(e => e.IdUser, "IXFK_UserToken_User");

		entityBuilder.Property(e => e.IdUserToken)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdLoginProvider).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdUser).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.Value)
			.IsRequired()
			.HasColumnType("nvarchar(4000)")
			.HasMaxLength(4000);

		entityBuilder.Property(e => e.Data).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.ModifiedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.ValidToUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.LastAccessUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.RemoteIP)
			.HasColumnType("nvarchar(64)")
			.HasMaxLength(64);

		entityBuilder.HasOne(d => d.LoginProvider)
			.WithMany(p => p.UserTokens)
			.HasForeignKey(d => d.IdLoginProvider)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_UserToken_IdLoginProvider");

		entityBuilder.HasOne(d => d.User)
			.WithMany(p => p.UserTokens)
			.HasForeignKey(d => d.IdUser)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_UserToken_IdUser");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Auth.Model.UserToken>(ConfigureEntity);

		return modelBuilder;
	}
}
