using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Logs.PostgreSQL;

public class LogConfiguration : IEntityTypeConfiguration<Logs.Model.Log>
{
	public const string PrimaryKeyFormatter = "{{\"IdLog\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Logs.Model.Log> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Logs.Model.Log> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdLog);

		entityBuilder.ToTable("Log", "log");

		entityBuilder.HasIndex(e => e.CorrelationId, "IX_Log_CorrelationId");

		entityBuilder.HasIndex(e => e.IdLogLevel, "IX_Log_IdLogLevel");

		entityBuilder.Property(e => e.IdLog)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.Component)
			.HasColumnType("varchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.OperationName)
			.HasColumnType("varchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.AggregateName)
			.HasColumnType("varchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.AggregateIdentifier)
			.HasColumnType("varchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.CustomCorrelationId)
			.HasColumnType("varchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.IdApplicationEntry).HasColumnType("uuid");

		entityBuilder.Property(e => e.CorrelationId).HasColumnType("uuid");

		entityBuilder.Property(e => e.ExternalCorrelationId)
			.HasColumnType("varchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.ContextProperties).HasColumnType("jsonb");

		entityBuilder.Property(e => e.IdUser).HasColumnType("uuid");

		entityBuilder.Property(e => e.TenantIdentifier).HasColumnType("uuid");

		entityBuilder.Property(e => e.LogCode)
			.HasColumnType("varchar(63)")
			.HasMaxLength(63);

		entityBuilder.Property(e => e.SourceSystemName)
			.HasColumnType("varchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uuid");

		entityBuilder.Property(e => e.RuntimeUniqueKey).HasColumnType("uuid");

		entityBuilder.Property(e => e.PropertyName)
			.HasColumnType("varchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.DisplayPropertyName)
			.HasColumnType("varchar(255)")
			.HasMaxLength(255);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Logs.Model.Log>(ConfigureEntity);

		return modelBuilder;
	}
}
