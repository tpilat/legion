using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Logs.PostgreSQL;

public class UnstructuredLogConfiguration : IEntityTypeConfiguration<Logs.Model.UnstructuredLog>
{
	public const string PrimaryKeyFormatter = "{{\"IdUnstructuredLog\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Logs.Model.UnstructuredLog> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Logs.Model.UnstructuredLog> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdUnstructuredLog);

		entityBuilder.ToTable("UnstructuredLog", "log");

		entityBuilder.HasIndex(e => e.IdLogLevel, "IX_UnstructuredLog_LogLevel");

		entityBuilder.Property(e => e.IdUnstructuredLog)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.RuntimeUniqueKey).HasColumnType("uuid");

		entityBuilder.Property(e => e.EventName)
			.HasColumnType("varchar(511)")
			.HasMaxLength(511);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Logs.Model.UnstructuredLog>(ConfigureEntity);

		return modelBuilder;
	}
}
