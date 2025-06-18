using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Auditing.PostgreSQL.Audit;

public class AuditTypeConfiguration : IEntityTypeConfiguration<Auditing.Audit.AuditType>
{
	public const string PrimaryKeyFormatter = "{{\"IdAuditType\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Auditing.Audit.AuditType> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Auditing.Audit.AuditType> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdAuditType);

		entityBuilder.ToTable("AuditType", "aud");

		entityBuilder.HasIndex(e => e.ItemCode, "UQ_AuditType_ItemCode")
				.IsUnique();

		entityBuilder.Property(e => e.IdAuditType).ValueGeneratedNever();

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasMaxLength(15);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasMaxLength(15);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Auditing.Audit.AuditType>(ConfigureEntity);

		return modelBuilder;
	}
}
