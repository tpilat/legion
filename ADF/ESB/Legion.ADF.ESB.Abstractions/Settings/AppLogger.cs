using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;

#nullable disable

namespace Legion.ADF.ESB.Settings;

public class AppLogger
{
	public string DBConnectionString { get; set; }
	public string LogFilePath { get; set; } = @"C:\Logs\ADF_ESB.json";
	public int? MinLogLevel { get; set; } = 0;
	public int ArchiveAboveSizeInBytes { get; set; } = 1048576;
	public int MaxArchiveFiles { get; set; } = 50;
	public string Encoding { get; set; } = "utf-8";
	public bool AutoEnableMetrics { get; set; }

	public class Validator : ValidatorBase<AppLogger>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<AppLogger> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<AppLogger> builder)
		{
			builder?
				.ForProperty(x => x.MinLogLevel, v => v.Range(0, true, 5, true))
				.ForProperty(x => x.ArchiveAboveSizeInBytes, v => v.GreaterThan(0))
				.ForProperty(x => x.MaxArchiveFiles, v => v.GreaterThan(0))
			;
		}
	}
}

public static class AppLoggerExtensions
{
	public static IServiceCollection AddAppLogger(this IServiceCollection services)
	{
		services
			.AddOptions<AppLogger>()
			.BindConfiguration("ADFESB:AppLogger")
			.AddOptionsValidator("ADFESB.AppLogger")
			.ValidateOnStart();

		return services;
	}
}
