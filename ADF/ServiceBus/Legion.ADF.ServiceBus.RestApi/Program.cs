using Legion.ADF.Logs.Extensions;
using Legion.ADF.ServiceBus.Extensions;
using Legion.AspNetCore.WebApi;
using Legion.Extensions;
using System.Globalization;
using System.Reflection;

namespace Legion.ADF.ServiceBus.RestApi;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Services.AddApplicationEntryScopeContext(sp =>
		{
			Microsoft.Extensions.Logging.ILogger? logger = null;
			var loggerFactory = sp.GetService<ILoggerFactory>();
			if (loggerFactory != null)
				logger = loggerFactory.CreateLogger<Program>();

			var scopeContext = ScopeContext.Create(
				"Legion.ADF.ServiceBus.RestApi",
				removePreviousSameMethodFrame: true,
				previousScopeContext: null,
				correlationId: GlobalContext.Instance.NewGuid(),
				principal: null,
				idUser: null,
				businessProcess: null, //TODO: nastavit pri application entry v controlleri / Handleri
				component: "Legion.ADF.ServiceBus.RestApi",
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

		var esbConfigBindingPath = "WebApi";
		builder.Services.AddADFServiceBusMonitor(esbConfigBindingPath, builder.Configuration)
			.ConfigurePostgreSQL();

		builder.Services.AddInMemoryMessageBus([
			typeof(ServiceBus.PostgreSQL.TableInfoProvider).Assembly,
			typeof(Program).Assembly
			]);

		Assembly[] assemblies = [
			typeof(Program).Assembly
		];

		//Add all validators from Legion.ADF.Logs.Abstractions.dll
		builder.Services.AddValidators(ServiceLifetime.Singleton, assemblies);

		//add all TOption builders
		builder.Services.ConfigureOptionsBuilders(assemblies);

		builder.Services.AddWebApiControllers(builder.Configuration, [ typeof(Legion.ADF.ServiceBus.AppDefaults).Assembly ], esbConfigBindingPath);

		var app = builder.Build();

		app.UseWebApi();

		//app.UseHttpsRedirection();

		app.UseAuthorization();

		app.MapControllers();

		app.Run();
	}
}
