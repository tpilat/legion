using Legion.Database.PostgreSQL.Extensions;
using Legion.EntityFrameworkCore.Database;
using Legion.EntityFrameworkCore.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Legion.ADF.ESB.Components.PostgreSQL.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddComponentsContexts(this IServiceCollection services)
	{
		services.AddLogging();
		services.AddPostgreSQLServices();
		services.AddPostgreSQLConnectionProvider<ConnectionStringProvider>();
		services.AddUnitOfWork<IComponentsUnitOfWork>(unitOfWorkContext => new ComponentsUnitOfWork(unitOfWorkContext));
		services.AddQueryUnitOfWork<IComponentsQueryUnitOfWork>(unitOfWorkContext => new ComponentsQueryUnitOfWork(unitOfWorkContext));
		services.AddDbContext<ComponentsDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		services.TryAddTransient<IComponentsDbContext, ComponentsDbContext>();
		services.AddDbContext<ComponentsQueryDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		services.TryAddTransient<IComponentsQueryDbContext, ComponentsQueryDbContext>();
		services.TryAddSingleton<Components.ITableInfoProvider, TableInfoProvider>();
		services.TryAddSingleton<Components.IQueryTableInfoProvider, QueryTableInfoProvider>();
		return services;
	}
}
