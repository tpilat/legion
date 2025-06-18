using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Audit.PostgreSQL;

public class ApplicationEntryTokenConfiguration : IEntityTypeConfiguration<Audit.Model.ApplicationEntryToken>
{
	public const string PrimaryKeyFormatter = "{{\"IdApplicationEntryToken\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Audit.Model.ApplicationEntryToken> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Audit.Model.ApplicationEntryToken> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdApplicationEntryToken);

		entityBuilder.ToTable("ApplicationEntryToken", "aud");

		entityBuilder.HasIndex(e => new { e.SourceFilePath, e.Token }, "UQ_ApplicationEntryToken_Token_SourceFilePath")
				.IsUnique();

		entityBuilder.Property(e => e.IdApplicationEntryToken)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Token)
			.IsRequired()
			.HasColumnType("varchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.SourceFilePath)
			.IsRequired()
			.HasColumnType("varchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.MethodInfo)
			.HasColumnType("varchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.AggregateName)
			.HasColumnType("varchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.Description)
			.HasColumnType("varchar(511)")
			.HasMaxLength(511);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Audit.Model.ApplicationEntryToken>(ConfigureEntity);

		return modelBuilder;
	}
}
