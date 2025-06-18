using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Auditing.PostgreSQL.Audit;

public class ApplicationEntryTokenConfiguration : IEntityTypeConfiguration<Auditing.Audit.ApplicationEntryToken>
{
	public const string PrimaryKeyFormatter = "{{\"IdApplicationEntryToken\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Auditing.Audit.ApplicationEntryToken> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Auditing.Audit.ApplicationEntryToken> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdApplicationEntryToken);

		entityBuilder.ToTable("ApplicationEntryToken", "aud");

		entityBuilder.HasIndex(e => new { e.SourceFilePath, e.Token, e.Version }, "UQ_ApplicationEntryToken_Token_Version_SourceFilePath")
				.IsUnique();

		entityBuilder.Property(e => e.IdApplicationEntryToken).ValueGeneratedNever();

		entityBuilder.Property(e => e.Token)
			.IsRequired()
			.HasMaxLength(255);

		entityBuilder.Property(e => e.SourceFilePath)
			.IsRequired()
			.HasMaxLength(511);

		entityBuilder.Property(e => e.MethodInfo).HasMaxLength(511);

		entityBuilder.Property(e => e.MainEntityName).HasMaxLength(255);

		entityBuilder.Property(e => e.Description).HasMaxLength(511);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Auditing.Audit.ApplicationEntryToken>(ConfigureEntity);

		return modelBuilder;
	}
}
