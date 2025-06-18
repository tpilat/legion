using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Logs.PostgreSQL;

public class LogLevelConfiguration : IEntityTypeConfiguration<Logs.Model.LogLevel>
{
	public const string PrimaryKeyFormatter = "{{\"IdLogLevel\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Logs.Model.LogLevel> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Logs.Model.LogLevel> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdLogLevel);

		entityBuilder.ToTable("LogLevel", "log");

		entityBuilder.HasIndex(e => e.ItemCode, "UQ_LogLevel_ItemCode")
				.IsUnique();

		entityBuilder.Property(e => e.IdLogLevel)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("varchar(31)")
			.HasMaxLength(31);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("varchar(31)")
			.HasMaxLength(31);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Logs.Model.LogLevel>(ConfigureEntity);

		return modelBuilder;
	}
}
