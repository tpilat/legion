using Legion.ADF.ESB.Components;
using Legion.ADF.ESB.Components.PostgreSQL;
using Legion.ADF.ESB.Components.PostgreSQL.Extensions;
using Legion.ADF.ESB.MBox.PostgreSQL.Extensions;
using Legion.ADF.ESB.Orchestrations.PostgreSQL.Extensions;
using Legion.ADF.ESB.ServiceBus.Extensions;
using Legion.ADF.ESB.ServiceBus.PostgreSQL;
using Legion.ADF.ESB.Settings;
using Legion.EntityFrameworkCore;
using Legion.Model.Audit;
using Legion;
using Legion.Extensions;
using Legion.MessageBus;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TestEnterpriseServiceBus.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Legion.Logging.PostgreSQL;
using Legion.ADF.ESB.ServiceBus.Initializers;

namespace TestEnterpriseServiceBus;

internal class Startup
{
	private readonly IConfiguration _configuration;

	public Startup(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public void ConfigureServices(IServiceCollection services)
	{
		services.AddLogging(builder =>
		{
			//builder.ClearProviders();
			builder.AddPostgreSQLLogger();
		});

		services.AddADFEngerpriseServiceBus(c => c.ConfigurePostgreSQL(services));

		services.AddInMemoryMessageBus([typeof(Startup).Assembly]);

		//Add all validators from Startup
		services.AddValidators<Startup>();



		//add all TOption builders
		services.ConfigureOptionsBuilders(typeof(Startup).Assembly);

		//add all service builders
		services.ConfigureServiceCollectionBuilders(_configuration, typeof(Startup).Assembly);

		services.AddStartupTask<ESBInitializer>();

		services.AddHostedService<TestWorker>();
	}
}
