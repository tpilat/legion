using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Audit.SqlServer;

public class VwApplicationEntryConfiguration : IEntityTypeConfiguration<Audit.Model.VwApplicationEntry>
{
	public void Configure(EntityTypeBuilder<Audit.Model.VwApplicationEntry> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Audit.Model.VwApplicationEntry> entityBuilder)
	{
		entityBuilder.ToView("VwApplicationEntry", "aud")
			.HasNoKey();

		entityBuilder.Property(e => e.IdApplicationEntry).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdApplicationEntryToken).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.Token)
			.IsRequired()
			.HasColumnType("nvarchar(255)");

		entityBuilder.Property(e => e.SourceFilePath)
			.IsRequired()
			.HasColumnType("nvarchar(511)");

		entityBuilder.Property(e => e.MethodInfo).HasColumnType("nvarchar(511)");

		entityBuilder.Property(e => e.AggregateName).HasColumnType("nvarchar(255)");

		entityBuilder.Property(e => e.AggregateIdentifier).HasColumnType("nvarchar(511)");

		entityBuilder.Property(e => e.Description).HasColumnType("nvarchar(511)");

		entityBuilder.Property(e => e.IdAuditOperation).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.RuntimeUniqueKey).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.CorrelationId).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.ExternalCorrelationId).HasColumnType("nvarchar(127)");

		entityBuilder.Property(e => e.HttpMethod).HasColumnType("nvarchar(15)");

		entityBuilder.Property(e => e.Uri).HasColumnType("nvarchar(1023)");

		entityBuilder.Property(e => e.IdUser).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.TenantIdentifier).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.RemoteIP).HasColumnType("nvarchar(63)");

		entityBuilder.Property(e => e.IdApplicationEntryRequest).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdApplicationEntryResponse).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.StatusCode).HasColumnType("varchar(63)");

		entityBuilder.Property(e => e.Error).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.ElapsedMilliseconds).HasColumnType("numeric(18, 0)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
		=> modelBuilder.Entity<Audit.Model.VwApplicationEntry>(ConfigureEntity);
}
