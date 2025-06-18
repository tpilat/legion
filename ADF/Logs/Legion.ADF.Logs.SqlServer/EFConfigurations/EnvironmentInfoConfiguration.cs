using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Logs.SqlServer;

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
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.ApplicationName)
			.HasColumnType("nvarchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.ApplicationVersion)
			.HasColumnType("nvarchar(15)")
			.HasMaxLength(15);

		entityBuilder.Property(e => e.RunningEnvironment)
			.HasColumnType("nvarchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.ProcessName)
			.HasColumnType("nvarchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.FrameworkDescription)
			.HasColumnType("nvarchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.TargetFramework)
			.HasColumnType("nvarchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.CLRVersion)
			.HasColumnType("nvarchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.EntryAssemblyName)
			.HasColumnType("nvarchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.EntryAssemblyVersion)
			.HasColumnType("nvarchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.BaseDirectory)
			.HasColumnType("nvarchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.MachineName)
			.HasColumnType("nvarchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.CurrentAppDomainName)
			.HasColumnType("nvarchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.Is64BitOperatingSystem).HasColumnType("bit");

		entityBuilder.Property(e => e.Is64BitProcess).HasColumnType("bit");

		entityBuilder.Property(e => e.OperatingSystemArchitecture)
			.HasColumnType("nvarchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.OperatingSystemPlatform)
			.HasColumnType("nvarchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.OperatingSystemVersion)
			.HasColumnType("nvarchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.ProcessArchitecture)
			.HasColumnType("nvarchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.CommandLine)
			.HasColumnType("nvarchar(1023)")
			.HasMaxLength(1023);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Logs.Model.EnvironmentInfo>(ConfigureEntity);

		return modelBuilder;
	}
}
