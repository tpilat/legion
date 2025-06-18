using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Logs.SqlServer;

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
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.InternalMessage).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.ClientMessage).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.Detail).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.StackTrace).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.Component)
			.HasColumnType("nvarchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.OperationName)
			.HasColumnType("nvarchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.AggregateName)
			.HasColumnType("nvarchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.AggregateIdentifier)
			.HasColumnType("nvarchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.CustomCorrelationId)
			.HasColumnType("nvarchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.IdApplicationEntry).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.CorrelationId).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.ExternalCorrelationId)
			.HasColumnType("nvarchar(511)")
			.HasMaxLength(511);

		entityBuilder.Property(e => e.ContextProperties).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.IdUser).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.TenantIdentifier).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.LogCode)
			.HasColumnType("nvarchar(63)")
			.HasMaxLength(63);

		entityBuilder.Property(e => e.SourceSystemName)
			.HasColumnType("nvarchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.TraceFrame).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.SourceContext).HasColumnType("nvarchar(max)");

		entityBuilder.Property(e => e.RuntimeUniqueKey).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IsValidationError).HasColumnType("bit");

		entityBuilder.Property(e => e.PropertyName)
			.HasColumnType("nvarchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.DisplayPropertyName)
			.HasColumnType("nvarchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.ValidationFailure).HasColumnType("nvarchar(max)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Logs.Model.Log>(ConfigureEntity);

		return modelBuilder;
	}
}
