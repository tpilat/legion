using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Logs.PostgreSQL;

public class EnvironmentInfoConfiguration : IEntityTypeConfiguration<Logs.Model.EnvironmentInfo>
{
	public const string PrimaryKeyFormatter = "{{\"IdEnvironmentInfo\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Logs.Model.EnvironmentInfo> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Logs.Model.EnvironmentInfo> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdEnvironmentInfo);

		entityBuilder.ToTable("EnvironmentInfo", "log");

		entityBuilder.Property(e => e.IdEnvironmentInfo)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.ApplicationName)
			.HasColumnType("varchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.ApplicationVersion)
			.HasColumnType("varchar(15)")
			.HasMaxLength(15);

		entityBuilder.Property(e => e.RunningEnvironment)
			.HasColumnType("varchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.ProcessName)
			.HasColumnType("varchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.FrameworkDescription)
			.HasColumnType("varchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.TargetFramework)
			.HasColumnType("varchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.CLRVersion)
			.HasColumnType("varchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.EntryAssemblyName)
			.HasColumnType("varchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.EntryAssemblyVersion)
			.HasColumnType("varchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.BaseDirectory)
			.HasColumnType("varchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.MachineName)
			.HasColumnType("varchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.CurrentAppDomainName)
			.HasColumnType("varchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.OperatingSystemArchitecture)
			.HasColumnType("varchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.OperatingSystemPlatform)
			.HasColumnType("varchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.OperatingSystemVersion)
			.HasColumnType("varchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.ProcessArchitecture)
			.HasColumnType("varchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.CommandLine)
			.HasColumnType("varchar(1023)")
			.HasMaxLength(1023);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Logs.Model.EnvironmentInfo>(ConfigureEntity);

		return modelBuilder;
	}
}
