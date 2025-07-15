using Legion.ADF.Logs.Extensions;
using Legion.ADF.ServiceBus.Extensions;
using Legion.Extensions;
using System.Globalization;

namespace Legion.ADF.ServiceBus.Service;

public class Program
{
	public static async Task Main(string[] args)
	{
		var builder = Host.CreateApplicationBuilder(args);

		builder.Services.AddApplicationEntryScopeContext(sp =>
		{
			Microsoft.Extensions.Logging.ILogger? logger = null;
			var loggerFactory = sp.GetService<ILoggerFactory>();
			if (loggerFactory != null)
				logger = loggerFactory.CreateLogger<Program>();

			var scopeContext = ScopeContext.Create(
				"Legion.ADF.ServiceBus SERVICE",
				removePreviousSameMethodFrame: true,
				previousScopeContext: null,
				correlationId: GlobalContext.Instance.NewGuid(),
				principal: null,
				idUser: null,
				businessProcess: null, //TODO: nastavit pri application entry v controlleri / Handleri
				component: "Legion.ADF.ServiceBus SERVICE",
				tenantIdentifier: null,
				externalCorrelationId: null,
				customCorrelationId: null,
				logger: logger,
				cultureInfo: new CultureInfo("sk"),
				requestMetadata: null,
				cancellationToken: null);

			return scopeContext;
		});

		builder.Services.AddADFLogs(builder.Configuration)
			.ConfigurePostgreSQL();

		var esbConfigBindingPath = "ESB";
		builder.Services.AddADFEnterpriseServiceBus(esbConfigBindingPath, builder.Configuration)
			.ConfigurePostgreSQL();

		builder.Services.AddInMemoryMessageBus([
			typeof(ServiceBus.PostgreSQL.TableInfoProvider).Assembly,
			typeof(Program).Assembly
			]);

		var windowsServiceName =
			builder.Configuration[$"{esbConfigBindingPath}:{nameof(Settings.EnterpriseServiceBusOptions)}:{nameof(Settings.EnterpriseServiceBusOptions.ServiceName)}"]
			?? $"ESB";

		//if OS != Windows does nothing
		builder.Services.AddWindowsService(options =>
		{
			options.ServiceName = windowsServiceName;
		});

		var host = builder.Build();
		await host.RunWithTasksAsync();
	}
}
