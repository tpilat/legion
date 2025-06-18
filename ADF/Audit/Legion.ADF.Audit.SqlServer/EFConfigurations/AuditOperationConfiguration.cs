using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Audit.SqlServer;

public class AuditOperationConfiguration : IEntityTypeConfiguration<Audit.Model.AuditOperation>
{
	public const string PrimaryKeyFormatter = "{{\"IdAuditOperation\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Audit.Model.AuditOperation> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Audit.Model.AuditOperation> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdAuditOperation);

		entityBuilder.ToTable("AuditOperation", "aud");

		entityBuilder.Property(e => e.IdAuditOperation)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("nvarchar(15)")
			.HasMaxLength(15);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("nvarchar(15)")
			.HasMaxLength(15);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Audit.Model.AuditOperation>(ConfigureEntity);

		return modelBuilder;
	}
}
