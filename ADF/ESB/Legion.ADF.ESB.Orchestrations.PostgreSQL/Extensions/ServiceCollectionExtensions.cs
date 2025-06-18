using Legion.Database.PostgreSQL.Extensions;
using Legion.EntityFrameworkCore.Database;
using Legion.EntityFrameworkCore.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Legion.ADF.ESB.Orchestrations.PostgreSQL.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddOrchestrationsContexts(this IServiceCollection services)
	{
		services.AddLogging();
		services.AddPostgreSQLServices();
		services.AddPostgreSQLConnectionProvider<ConnectionStringProvider>();
		services.AddUnitOfWork<IOrchestrationsUnitOfWork>(unitOfWorkContext => new OrchestrationsUnitOfWork(unitOfWorkContext));
		services.AddQueryUnitOfWork<IOrchestrationsQueryUnitOfWork>(unitOfWorkContext => new OrchestrationsQueryUnitOfWork(unitOfWorkContext));
		services.AddDbContext<OrchestrationsDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		services.TryAddTransient<IOrchestrationsDbContext, OrchestrationsDbContext>();
		services.AddDbContext<OrchestrationsQueryDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		services.TryAddTransient<IOrchestrationsQueryDbContext, OrchestrationsQueryDbContext>();
		services.TryAddSingleton<Orchestrations.ITableInfoProvider, TableInfoProvider>();
		services.TryAddSingleton<Orchestrations.IQueryTableInfoProvider, QueryTableInfoProvider>();
		return services;
	}
}
