using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;

#nullable disable

namespace Legion.ADF.ServiceBus.Settings;

public class AppSettings
{
	private static bool _initialized;
	public static AppSettings Instance { get; private set; }
	public static string ApplicationVersion { get; private set; }
	public static string CurrentDirectory { get; private set; }

	public string DefaultCultureName { get; set; } = "sk-SK";
	public string DefaultCulture { get; set; } = "sk";
	//public AppLogger AppLogger { get; }

	public AppSettings()
	{
		//AppLogger = new AppLogger();
	}

	private static readonly object _initLock = new();
	internal static void Initialize(
		AppSettings instance,
		string applicationVersion,
		string currentDirectory)
	{
		if (_initialized)
			return;

		lock (_initLock)
		{
			if (_initialized)
				return;

			Throw.IfArgumentNull(instance);
			Throw.IfArgumentNullOrWhiteSpace(applicationVersion);
			Throw.IfArgumentNullOrWhiteSpace(currentDirectory);

			_initialized = true;
			Instance = instance;
			ApplicationVersion = applicationVersion;
			CurrentDirectory = currentDirectory;
		}
	}

	public class Validator : ValidatorBase<AppSettings>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<AppSettings> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<AppSettings> builder)
		{
			builder?
				.ForProperty(x => x.DefaultCultureName, v => v.NotDefaultOrWhiteSpace())
				.ForProperty(x => x.DefaultCulture, v => v.NotDefaultOrWhiteSpace())
				//.ForProperty(x => x.AppLogger, v => v.NotNull())
				//.ForNavigation(
				//	x => x.AppLogger,
				//	Settings.AppLogger.Validator.RulesBuilder)
			;
		}
	}
}

public static class AppSettingsExtensions
{
	public static IServiceCollection AddAppSettings(
		this IServiceCollection services,
		string esbConfigBindingPath,
		bool service)
	{
		services
			.AddOptions<AppSettings>()
			.BindConfiguration($"{esbConfigBindingPath}")
			.Configure(o =>
			{
				AppSettings.Initialize(
					o,
					typeof(AppSettings).Assembly.GetName().Version!.ToString(),
					System.AppDomain.CurrentDomain.BaseDirectory);
			})
			.AddOptionsValidator($"{esbConfigBindingPath}.{nameof(AppSettings)}")
			.ValidateOnStart();

		services.AddDBSettings(esbConfigBindingPath);

		if (service)
			services.AddEnterpriseServiceBusOptions(esbConfigBindingPath);
		else
			services.AddServiceBusMonitorOptions(esbConfigBindingPath);

			return services;
	}
}
