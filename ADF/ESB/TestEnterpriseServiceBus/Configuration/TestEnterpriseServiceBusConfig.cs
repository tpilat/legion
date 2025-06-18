using Legion;
using Legion.DependencyInjection;
using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;
using TestEnterpriseServiceBus.Adapters.DB;
using TestEnterpriseServiceBus.Adapters.RPO;
using TestEnterpriseServiceBus.Adapters.SocPoist;

namespace TestEnterpriseServiceBus.Configuration;

public class TestEnterpriseServiceBusConfig
{
	private static bool _initialized;
	public static TestEnterpriseServiceBusConfig Instance { get; private set; }
	public static string ApplicationVersion { get; private set; }
	public static string CurrentDirectory { get; private set; }

	private static readonly object _initLock = new();
	internal static void Initialize(
		TestEnterpriseServiceBusConfig instance,
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

	public class Validator : ValidatorBase<TestEnterpriseServiceBusConfig>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<TestEnterpriseServiceBusConfig> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<TestEnterpriseServiceBusConfig> builder)
		{
		}
	}
}

public class TestEnterpriseServiceBusConfigExtensions : IServiceCollectionOptionsBuilder
{
	public static IServiceCollection ConfigureOptions(IServiceCollection services)
	{
		services
			.AddOptions<TestEnterpriseServiceBusConfig>()
			.BindConfiguration("TestEnterpriseServiceBusConfig")
			.Configure(o =>
			{
				TestEnterpriseServiceBusConfig.Initialize(
					o,
					typeof(TestEnterpriseServiceBusConfig).Assembly.GetName().Version!.ToString(),
					System.AppDomain.CurrentDomain.BaseDirectory);
			})
			.AddOptionsValidator("TestEnterpriseServiceBusConfig")
			.ValidateOnStart();

		services
			.AddOptions<SocPoistDBAdapterConfig>()
			//.BindConfiguration("SocPoistDBAdapterConfig")
			.Configure<IServiceProvider>((o, sp) =>
			{
				o.StoreId = "ADF_ESB";
				o.TimeoutInSeconds = 60;
			})
			.AddOptionsValidator("SocPoistDBAdapterConfig")
			//.ValidateOnStart()
			;

		return services;
	}
}
