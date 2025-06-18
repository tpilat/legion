using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Auth.SqlServer;

public class LoginProviderConfiguration : IEntityTypeConfiguration<Auth.Model.LoginProvider>
{
	public const string PrimaryKeyFormatter = "{{\"IdLoginProvider\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Auth.Model.LoginProvider> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Auth.Model.LoginProvider> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdLoginProvider);

		entityBuilder.ToTable("LoginProvider", "auth");

		entityBuilder.HasIndex(e => e.Code, "UQ_LoginProvider_Code")
				.IsUnique();

		entityBuilder.Property(e => e.IdLoginProvider)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("nvarchar(128)")
			.HasMaxLength(128);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("nvarchar(128)")
			.HasMaxLength(128);

		entityBuilder.Property(e => e.DisabledUtc).HasColumnType("datetime2(7)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Auth.Model.LoginProvider>(ConfigureEntity);

		return modelBuilder;
	}
}
