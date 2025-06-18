using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Audit.PostgreSQL;

public class VwApplicationEntryConfiguration : IEntityTypeConfiguration<Audit.Model.VwApplicationEntry>
{
	public void Configure(EntityTypeBuilder<Audit.Model.VwApplicationEntry> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Audit.Model.VwApplicationEntry> entityBuilder)
	{
		entityBuilder.ToView("VwApplicationEntry", "aud")
			.HasNoKey();

		entityBuilder.Property(e => e.IdApplicationEntry).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdApplicationEntryToken).HasColumnType("uuid");

		entityBuilder.Property(e => e.Token)
			.IsRequired()
			.HasColumnType("varchar(255)");

		entityBuilder.Property(e => e.SourceFilePath)
			.IsRequired()
			.HasColumnType("varchar(511)");

		entityBuilder.Property(e => e.MethodInfo).HasColumnType("varchar(511)");

		entityBuilder.Property(e => e.AggregateName).HasColumnType("varchar(255)");

		entityBuilder.Property(e => e.AggregateIdentifier).HasColumnType("varchar(511)");

		entityBuilder.Property(e => e.Description).HasColumnType("varchar(511)");

		entityBuilder.Property(e => e.IdAuditOperation).HasColumnType("uuid");

		entityBuilder.Property(e => e.RuntimeUniqueKey).HasColumnType("uuid");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.CorrelationId).HasColumnType("uuid");

		entityBuilder.Property(e => e.ExternalCorrelationId).HasColumnType("varchar(127)");

		entityBuilder.Property(e => e.HttpMethod).HasColumnType("varchar(15)");

		entityBuilder.Property(e => e.Uri).HasColumnType("varchar(1023)");

		entityBuilder.Property(e => e.IdUser).HasColumnType("uuid");

		entityBuilder.Property(e => e.TenantIdentifier).HasColumnType("uuid");

		entityBuilder.Property(e => e.RemoteIP).HasColumnType("varchar(63)");

		entityBuilder.Property(e => e.IdApplicationEntryRequest).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdApplicationEntryResponse).HasColumnType("uuid");

		entityBuilder.Property(e => e.StatusCode).HasColumnType("varchar(63)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Audit.Model.VwApplicationEntry>(ConfigureEntity);
}
