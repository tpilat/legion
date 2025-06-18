using Legion.ADF.ServiceBus.Orchestrations.PostgreSQL;
using Legion.Database.PostgreSQL.Extensions;
using Legion.EntityFrameworkCore.Database;
using Legion.EntityFrameworkCore.Extensions;
using Legion.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Legion.ADF.ServiceBus.Orchestrations.Extensions;

public static class ServiceCollectionExtensions
{
	public static ADFServiceBusBuilder ConfigureOrchestrationsPostgreSQL(this ADFServiceBusBuilder builder)
	{
		Throw.IfArgumentNull(builder);

		builder.Services.AddInMemoryMessageBus([typeof(OrchestrationsDbContext).Assembly]);

		builder.Services.AddLogging();
		builder.Services.AddPostgreSQLServices();
		builder.Services.AddPostgreSQLConnectionProvider<Legion.ADF.ServiceBus.ConnectionStringProvider>();
		builder.Services.AddUnitOfWork<IOrchestrationsUnitOfWork>(efConnectionProvider => new OrchestrationsUnitOfWork(efConnectionProvider));
		builder.Services.AddQueryUnitOfWork<IOrchestrationsQueryUnitOfWork>(efConnectionProvider => new OrchestrationsQueryUnitOfWork(efConnectionProvider));
		builder.Services.TryAddSingleton<IOrchestrationsUnitOfWorkFactory, OrchestrationsUnitOfWorkFactory>();
		builder.Services.TryAddSingleton<IOrchestrationsQueryUnitOfWorkFactory, OrchestrationsQueryUnitOfWorkFactory>();
		builder.Services.AddDbContext<OrchestrationsDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.Services.TryAddTransient<IOrchestrationsDbContext, OrchestrationsDbContext>();
		builder.Services.AddDbContext<OrchestrationsQueryDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.Services.TryAddTransient<IOrchestrationsQueryDbContext, OrchestrationsQueryDbContext>();
		builder.Services.TryAddSingleton<Orchestrations.ITableInfoProvider, TableInfoProvider>();
		builder.Services.TryAddSingleton<Orchestrations.IQueryTableInfoProvider, QueryTableInfoProvider>();

		return builder;
	}
}
